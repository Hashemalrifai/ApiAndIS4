using Microsoft.AspNetCore.Mvc;

namespace ApiAndIS4.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly AppDbContext appDbContext;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, AppDbContext appDbContext)
    {
        _logger = logger;
        this.appDbContext = appDbContext;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return appDbContext.WeatherForecasts.ToList();
    }
}

