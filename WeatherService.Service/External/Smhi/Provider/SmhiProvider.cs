using Microsoft.Extensions.Options;
using System.Reflection.Metadata;
using System.Text.Json;
using WeatherService.Contracts.WeatherForecast;
using WeatherService.Service.External.Smhi.Models;

namespace WeatherService.Service.External.Smhi.Provider;

public class SmhiProvider : ISmhiProvider
{
    private readonly HttpClient _httpClient;
    private readonly SmhiApiOptions _options;
    private readonly JsonSerializerOptions _jsonOptions;

    public SmhiProvider(HttpClient httpClient, IOptions<SmhiApiOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        _httpClient.BaseAddress = new Uri(_options.BaseUrl);
    }

    public async Task<SmhiStationData> GetStationTemperatureDataAsync(string station, string period)
    {
        return await GetStationDataAsync(SmhiProviderMagicStrings.TemperatureParameter, station, period);
    }
    
    public async Task<SmhiStationData> GetStationWindDataAsync(string station, string period)
    {
        return await GetStationDataAsync(SmhiProviderMagicStrings.WindParameter, station, period);
    }

    private async Task<SmhiStationData> GetStationDataAsync(string parameter, string station, string period, string version = SmhiProviderMagicStrings.DefaultApiVersion)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/version/{version}/parameter/{parameter}/station/{station}/period/{period}/data.json");
            response.EnsureSuccessStatusCode();

            var jsonContent = await response.Content.ReadAsStringAsync();
            var observationData = JsonSerializer.Deserialize<SmhiStationData>(jsonContent, _jsonOptions);

            return observationData ?? new SmhiStationData();
        }
        catch (Exception ex)
        {
            throw new HttpRequestException($"Error fetching SMHI observation data: {ex.Message}", ex);
        }
    }

    public async Task<SmhiStationSetData> GetStationSetTemperatureDataAsync()
    {
        return await GetStationSetDataAsync(SmhiProviderMagicStrings.TemperatureParameter);
    }
    
    public async Task<SmhiStationSetData> GetStationSetWindDataAsync()
    {
        return await GetStationSetDataAsync(SmhiProviderMagicStrings.WindParameter);
    }
    
    private async Task<SmhiStationSetData> GetStationSetDataAsync(string parameter, string version = SmhiProviderMagicStrings.DefaultApiVersion)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/version/{version}/parameter/{parameter}/station-set/all/period/latest-hour/data.json");
            response.EnsureSuccessStatusCode();

            var jsonContent = await response.Content.ReadAsStringAsync();
            var observationData = JsonSerializer.Deserialize<SmhiStationSetData>(jsonContent, _jsonOptions);

            return observationData ?? new SmhiStationSetData();
        }
        catch (Exception ex)
        {
            throw new HttpRequestException($"Error fetching SMHI observation data: {ex.Message}", ex);
        }
    }
}
