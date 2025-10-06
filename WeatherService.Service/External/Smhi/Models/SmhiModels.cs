namespace WeatherService.Service.External.Smhi.Models;

public class SmhiStationSetData
{
    public List<SmhiStationSetDetails> Station { get; set; } = [];
}
public class SmhiStationSetDetails
{
    public string Name { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public List<SmhiValue> Value { get; set; } = [];
}

public class SmhiStationData
{
    public SmhiStationDetails Station { get; set; } = new();
    public List<SmhiValue> Value { get; set; } = [];
}

public class SmhiStationDetails
{
    public string Name { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
}

public class SmhiValue
{
    public long Date { get; set; }
    public string Value { get; set; } = string.Empty;
    public string Quality { get; set; } = string.Empty;

    public DateTime DateTimeUtc => DateTimeOffset.FromUnixTimeMilliseconds(Date).UtcDateTime;
}



