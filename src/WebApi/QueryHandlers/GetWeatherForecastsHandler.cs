namespace Velma.WebApi.QueryHandlers;

using Velma.WebApi.Interfaces;
using Velma.WebApi.Queries;
using Velma.WebApi.QueryResults;

internal sealed class GetWeatherForecastsHandler : IRequestHandler<GetWeatherForecasts, IEnumerable<WeatherForecast>>
{
    private readonly IWeatherForecastService service;

    public GetWeatherForecastsHandler(IWeatherForecastService service) => this.service = service;

    public async Task<IEnumerable<WeatherForecast>> Handle(GetWeatherForecasts request, CancellationToken cancellationToken)
    {
        return (await service.GetWeatherForecastsAsync(cancellationToken)).Select(forecast => (WeatherForecast)forecast);
    }
}
