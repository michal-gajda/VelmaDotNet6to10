namespace Velma.WebApi.Extensions;

using Source = Velma.WebApi.QueryResults.WeatherForecast;
using Target = Velma.WebApi.Results.WeatherForecast;

internal static class WeatherForecastExtensions
{
    public static Target ToResult(this Source source) => new()
    {
        Date = source.Date,
        TemperatureC = source.TemperatureC,
        Summary = source.Summary,
    };
}
