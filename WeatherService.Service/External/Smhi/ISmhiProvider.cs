using WeatherService.Service.External.Smhi.Models;

namespace WeatherService.Service.External.Smhi;

public interface ISmhiProvider
{
    /// <summary>
    /// Gets temperature data for a specific station
    /// </summary>
    Task<SmhiStationData> GetStationTemperatureDataAsync(string station, string period);
    
    /// <summary>
    /// Gets wind data for a specific station
    /// </summary>
    Task<SmhiStationData> GetStationWindDataAsync(string station, string period);

    /// <summary>
    /// Gets temperature data for all stations
    /// </summary>
    Task<SmhiStationSetData> GetStationSetTemperatureDataAsync();
    
    /// <summary>
    /// Gets wind data for all stations
    /// </summary>
    Task<SmhiStationSetData> GetStationSetWindDataAsync();
}
