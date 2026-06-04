using Microsoft.OpenApi.Models;

namespace Velma.WebApi;

public sealed class Program
{
    private Program()
    {
    }

    public static async Task<int> Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

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
                Format = "date"
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
