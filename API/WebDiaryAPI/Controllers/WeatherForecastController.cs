using Microsoft.AspNetCore.Mvc;

namespace WebDiaryAPI.Controllers;

[ApiController]
[Route("[controller]")]
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

    // [HttpGet(Name = "GetWeatherForecast")] -> sets the name of this endpoint, if we want to use this endpoint we need to call it using this name
    [HttpGet(Name = "GetWeatherForecast")]
    public WeatherForecast Get(string location)
    {
        var random = Random.Shared.Next(Summaries.Count());

        return new WeatherForecast
        {
            Day = DateTime.Now.ToString("dd-MM-yyyy"),
            Temperature = $"{Random.Shared.Next(-20, 55)} ºC",
            Wind = $"{Random.Shared.Next(1, 250)} km/h",
            Description = Summaries[random],
            Forecast = Enumerable.Range(1, 5).Select(index => new Forecast
            {
                Day = $"{index}",
                Temperature = $"{Random.Shared.Next(-20, 55)} ºC",
                Wind = $"{Random.Shared.Next(1, 250)} km/h"
            }).ToArray()
        };
    }
}