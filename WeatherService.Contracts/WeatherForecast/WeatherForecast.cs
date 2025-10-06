namespace WeatherService.Contracts.WeatherForecast;

public enum Period
{
    LatestHour,
    LatestDay,
}

public class WeatherForecastResponse
{
    public List<Station> Station { get; set; } = [];
}

public class Station
{
    public string Name { get; set; } = string.Empty;
    public List<WeatherData> WeatherData { get; set; } = [];
}

public class WeatherData
{
    public DateTime Date { get; set; }
    public string? TemperatureC { get; set; }
    public string? WindSpeed { get; set; }
}
