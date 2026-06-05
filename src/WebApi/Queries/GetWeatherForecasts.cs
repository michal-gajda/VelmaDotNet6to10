namespace Velma.WebApi.Queries;

using Velma.WebApi.QueryResults;

internal sealed record class GetWeatherForecasts : IRequest<IEnumerable<WeatherForecast>>
{
}
