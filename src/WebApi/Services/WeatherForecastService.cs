namespace Velma.WebApi.Services;

using Velma.WebApi.Interfaces;
using Velma.WebApi.Models;

internal sealed class WeatherForecastService : IWeatherForecastService
{
    private static readonly string[] Summaries =
    [
        "Balmy",
        "Bracing",
        "Chilly",
        "Cool",
        "Freezing",
        "Hot",
        "Mild",
        "Scorching",
        "Sweltering",
        "Warm",
    ];

    private readonly TimeProvider timeProvider;

    public WeatherForecastService(TimeProvider timeProvider) => this.timeProvider = timeProvider;

    public Task<IEnumerable<WeatherForecast>> GetWeatherForecastsAsync(int range, CancellationToken cancellationToken = default)
    {
        var dateTime = timeProvider.GetUtcNow().Date;

        var result = Enumerable.Range(1, range).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(dateTime.AddDays(index)),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)],
            TemperatureC = Random.Shared.Next(-20, 55),
        });

        return Task.FromResult(result);
    }
}
