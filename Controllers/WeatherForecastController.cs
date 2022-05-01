using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiAndIS4.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly AppDbContext appDbContext;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, AppDbContext appDbContext)
    {
        _logger = logger;
        this.appDbContext = appDbContext;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> GetWeatherForecast()
    {
        return appDbContext.WeatherForecasts.ToList();
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes ="Bearer")]
    public IEnumerable<WeatherForecast> GetWeatherForecastAuthorized()
    {
        return appDbContext.WeatherForecasts.ToList();
    }
}

