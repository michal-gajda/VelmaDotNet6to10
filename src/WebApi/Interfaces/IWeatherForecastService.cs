namespace Velma.WebApi.Interfaces;

using Velma.WebApi.Models;

internal interface IWeatherForecastService
{
    Task<IEnumerable<WeatherForecast>> GetWeatherForecastsAsync(CancellationToken cancellationToken = default);
}
