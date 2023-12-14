using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace RateLimit.Controllers;

[ApiController]
[Route("[controller]")]
[EnableRateLimiting("fixed")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    [EnableRateLimiting("tokenbucket")]
    public IEnumerable<WeatherForecast> GetAll()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
    
    
    // [HttpGet("{id:int}",Name = "GetWeather")]
    // [EnableRateLimiting("sliding")]
    // public IActionResult GetSingle()
    // {
    //     return Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
    //         {
    //             Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
    //             TemperatureC = Random.Shared.Next(-20, 55),
    //             Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    //         })
    //         .ToArray().FirstOrDefault());
    // }
    //
    [HttpGet("{id:int}",Name = "GetConcurrentWeather")]
    [EnableRateLimiting("concurrency")]
    public IActionResult GetConSingle()
    {
        return Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray().FirstOrDefault());
    }
}