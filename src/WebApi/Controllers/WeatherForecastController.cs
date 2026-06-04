namespace Velma.WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public sealed class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
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
    };

    private readonly ILogger<WeatherForecastController> logger;
    private readonly TimeProvider timeProvider;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, TimeProvider timeProvider) =>
        (this.logger, this.timeProvider) = (logger, timeProvider);

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        var dateTime = timeProvider.GetUtcNow().Date;

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(dateTime.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
