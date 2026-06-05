namespace Velma.WebApi;

using Microsoft.OpenApi.Models;
using Velma.WebApi.Interfaces;
using Velma.WebApi.Services;
using System.Reflection;

public sealed class Program
{
    private Program()
    {
    }

    public static async Task<int> Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

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
