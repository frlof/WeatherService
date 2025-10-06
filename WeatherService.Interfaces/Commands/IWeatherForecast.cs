using WeatherService.Contracts.WeatherForecast;

namespace WeatherService.Interfaces.Commands;

public interface IWeatherForecast
{
    Task<WeatherForecastResponse> GetWeatherForecastAsync(string? stationId, Period? period);
}

