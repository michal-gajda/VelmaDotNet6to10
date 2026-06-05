namespace Velma.WebApi.QueryResults;

using Source = Velma.WebApi.Models.WeatherForecast;

internal sealed record class WeatherForecast
{
    public required DateOnly Date { get; init; }
    public required int TemperatureC { get; init; }
    public string? Summary { get; init; }

    public static explicit operator WeatherForecast(Source source) => new()
    {
        Date = source.Date,
        TemperatureC = source.TemperatureC,
        Summary = source.Summary,
    };
}
