using WeatherService.Interfaces.Commands;
using WeatherService.Contracts.WeatherForecast;
using WeatherService.Service.External.Smhi;

namespace WeatherService.Service.Commands;

public class GetWeatherForecast(ISmhiProvider smhiProvider) : IWeatherForecast
{
    public async Task<WeatherForecastResponse> GetWeatherForecastAsync(string? stationId, Period? period)
    {
        return stationId == null && period == null ?
            await GetStationSetWeatherForecastAsync() 
            : await GetStationWeatherForecastAsync(stationId, period);
    }

    private async Task<WeatherForecastResponse> GetStationWeatherForecastAsync(string? stationId, Period? period)
    {
        var stationTemperatureData = await smhiProvider.GetStationTemperatureDataAsync(stationId, ApiPeriodToSmhiPeriod(period));
        var stationWindData = await smhiProvider.GetStationWindDataAsync(stationId, ApiPeriodToSmhiPeriod(period));
        
        var stationTemperatureDataDictionary = stationTemperatureData?.Value.ToDictionary(d => d.DateTimeUtc, d => d.Value);
        var stationWindDataKeysDictionary = stationTemperatureData?.Value.ToDictionary(d => d.DateTimeUtc, d => d.Value);

        var allDates = stationTemperatureDataDictionary.Keys.Union(stationWindDataKeysDictionary.Keys).OrderByDescending(d => d).ToList();

        var mergedWeatherData = new List<WeatherData>();
        foreach (var datepoint in allDates)
        {
            var tempExists = stationTemperatureDataDictionary.TryGetValue(datepoint, out var temperature);
            var windExists = stationWindDataKeysDictionary.TryGetValue(datepoint, out var wind);

            mergedWeatherData.Add(new WeatherData
            {
                Date = datepoint,
                TemperatureC = tempExists ? temperature : null,
                WindSpeed = windExists ? wind : null
            });
        }

        return new WeatherForecastResponse
        {
            Station =
            [
                new Station
                {
                    Name = stationTemperatureData?.Station?.Name ?? "Unknown Station",
                    WeatherData = mergedWeatherData
                }
            ]
        };
    }

    private async Task<WeatherForecastResponse> GetStationSetWeatherForecastAsync()
    {
        var stationSetTemperatureData = await smhiProvider.GetStationSetTemperatureDataAsync();
        var stationSetWindData = await smhiProvider.GetStationSetWindDataAsync();

        var stationTemperatureDataDictionary = stationSetTemperatureData.Station.ToDictionary(d => d.Key, d => d);
        var stationWindDataKeysDictionary = stationSetWindData.Station.ToDictionary(d => d.Key, d => d);

        var allStationKeys = stationTemperatureDataDictionary.Keys.Union(stationWindDataKeysDictionary.Keys).OrderByDescending(d => d).ToList();

        var mergedStationData = new List<Station>();
        foreach (var stationKey in allStationKeys)
        {
            var tempExists = stationTemperatureDataDictionary.TryGetValue(stationKey, out var temperature);
            var windExists = stationWindDataKeysDictionary.TryGetValue(stationKey, out var wind);

            var addTemp = tempExists && temperature.Value.Any();
            var addWind = windExists && wind.Value.Any();
            
            var station = new Station
            {
                Name = temperature?.Name ?? wind?.Name ?? string.Empty,
                WeatherData = []
            };

            if (station.Name == string.Empty) throw new Exception("Station name cannot be empty");

            var allDates = new List<DateTime?>()
            {
                addTemp ? temperature.Value.Single().DateTimeUtc : null,
                addWind ? wind.Value.Single().DateTimeUtc : null
            }
            .Where(d => d.HasValue)
            .Select(d => d.Value)
            .Distinct()
            .OrderByDescending(d => d);

            foreach (var date in allDates)
            {
                station.WeatherData.Add(new WeatherData
                {
                    Date = date,
                    TemperatureC = addTemp ? temperature.Value.Single()?.Value : null,
                    WindSpeed = addWind ? wind.Value.Single()?.Value : null
                });
            }
            mergedStationData.Add(station);
        }
            
        return new WeatherForecastResponse
        {
            Station = mergedStationData
        };
    }

    private static string ApiPeriodToSmhiPeriod(Period? period)
    {
        return period switch
        {
            Period.LatestHour => "latest-hour",
            Period.LatestDay => "latest-day",
            _ => throw new NotImplementedException(),

        };
    }
}
