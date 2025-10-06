using WeatherService.Service.External.Smhi.Models;

namespace WeatherService.Service.External.Smhi;

public interface ISmhiProvider
{
    Task<SmhiStationData> GetStationTemperatureDataAsync(string station, string period);
    Task<SmhiStationData> GetStationWindDataAsync(string station, string period);

    Task<SmhiStationSetData> GetStationSetTemperatureDataAsync();
    Task<SmhiStationSetData> GetStationSetWindDataAsync();
}
