namespace Velma.WebApi;

using Microsoft.OpenApi.Models;
using Velma.WebApi.Interfaces;
using Velma.WebApi.Services;
using System.Reflection;
using OpenTelemetry.Resources;
using OpenTelemetry.Logs;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;

public sealed class Program
{
    private const string SERVICE_NAME = "Velma.WebApi";
    private const string SERVICE_NAMESPACE = "Velma";
    private const string SERVICE_VERSION = "1.0.0";
    private const string SERVICE_INSTANCE_ID = "instance-1";

    private Program()
    {
    }

    public static async Task<int> Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHealthChecks();

        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(SERVICE_NAME, SERVICE_NAMESPACE, SERVICE_VERSION, autoGenerateServiceInstanceId: false, serviceInstanceId: SERVICE_INSTANCE_ID);

        builder.Logging.AddOpenTelemetry(options =>
        {
            options.AddOtlpExporter();
            options.IncludeFormattedMessage = true;
            options.IncludeScopes = true;
            options.ParseStateValues = true;
            options.SetResourceBuilder(resourceBuilder);
        });

        builder.Services.AddOpenTelemetry()
            .WithTracing(tracing => tracing
                .SetResourceBuilder(resourceBuilder)
                .SetSampler(new AlwaysOnSampler())
                .AddAspNetCoreInstrumentation(options =>
                {
                    options.RecordException = true;
                    options.Filter = context => !context.Request.Path.StartsWithSegments("/health");
                })
                .AddHttpClientInstrumentation(options => options.RecordException = true)
                .AddOtlpExporter())
            .WithMetrics(metrics => metrics
                .SetResourceBuilder(resourceBuilder)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation()
                .AddOtlpExporter());

        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        builder.Services.AddSingleton<IWeatherForecastService, WeatherForecastService>();

        builder.Services.AddControllers().AddJsonOptions(options =>
        {
#if NET6_0
            options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
#endif
        });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.MapType<DateOnly>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "date",
            });
        });

        builder.Services.AddSingleton(TimeProvider.System);

        var app = builder.Build();

        app.UseHealthChecks("/healthz");

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.MapControllers();

        await app.RunAsync();

        return Environment.ExitCode;
    }
}
