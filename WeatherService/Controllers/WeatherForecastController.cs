using Microsoft.AspNetCore.Mvc;
using WeatherService.Contracts.WeatherForecast;
using WeatherService.Interfaces.Commands;

namespace WeatherService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecast weatherForecast) : ControllerBase
    {

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get(
            [FromQuery] string? stationId,
            [FromQuery] Period? period
        )
        {
            if((stationId is null) != (period is null))
            {
                return BadRequest("StationId and period must be provided together");
            }
            try
            {
                var result = await weatherForecast.GetWeatherForecastAsync(stationId, period);
                return Ok(result);
            }
            catch
            {
                return BadRequest("Something went wrong");
            }
        }
    }
}
