namespace Velma.WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using Velma.WebApi.Results;
using Velma.WebApi.Queries;
using Velma.WebApi.Extensions;

[ApiController]
[Route("[controller]")]
public sealed class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> logger;
    private readonly IMediator mediator;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator) =>
        (this.logger, this.mediator) = (logger, mediator);

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> GetWeathersAsync(CancellationToken cancellationToken = default)
    {
        var query = new GetWeatherForecasts
        {
        };

        var result = (await mediator.Send(query, cancellationToken)).Select(forecast => forecast.ToResult());

        return result;
    }
}
