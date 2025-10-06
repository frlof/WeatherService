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
            logger.LogInformation("GetWeatherForecast endpoint called with StationId: {StationId}, Period: {Period}", stationId, period);

            if((stationId is null) != (period is null))
            {
                logger.LogWarning("Invalid parameter combination - StationId and period must be provided together. StationId: {StationId}, Period: {Period}", stationId, period);
                return BadRequest("StationId and period must be provided together");
            }

            try
            {
                logger.LogDebug("Calling weather forecast service with StationId: {StationId}, Period: {Period}", stationId, period);
                var result = await weatherForecast.GetWeatherForecastAsync(stationId, period);
                logger.LogInformation("Successfully retrieved weather forecast for StationId: {StationId}, Period: {Period}", stationId, period);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving weather forecast for StationId: {StationId}, Period: {Period}", stationId, period);
                return BadRequest("Something went wrong");
            }
        }
    }
}
