using WeatherService.Contracts.WeatherForecast;
using WeatherService.Service.Commands;
using WeatherService.Service.External.Smhi;
using WeatherService.Service.External.Smhi.Models;

namespace WeatherService.UnitTests;

public class GetWeatherForecastTests
{
    private readonly Mock<ISmhiProvider> _mockSmhiProvider;
    private readonly GetWeatherForecast _service;

    public GetWeatherForecastTests()
    {
        _mockSmhiProvider = new Mock<ISmhiProvider>();
        _service = new GetWeatherForecast(_mockSmhiProvider.Object);
    }

    [Fact]
    public async Task GetWeatherForecastAsync_WithNoParameters_ReturnsStationSetData()
    {
        // Arrange
        var temperatureData = CreateStationSetData("Station1", "20.5", "station-key-1");
        var windData = CreateStationSetData("Station1", "5.2", "station-key-1");

        _mockSmhiProvider.Setup(x => x.GetStationSetTemperatureDataAsync())
            .ReturnsAsync(temperatureData);
        _mockSmhiProvider.Setup(x => x.GetStationSetWindDataAsync())
            .ReturnsAsync(windData);

        // Act
        var result = await _service.GetWeatherForecastAsync(null, null);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Station);
        Assert.Equal("Station1", result.Station.Single().Name);
        Assert.Single(result.Station.Single().WeatherData);
        Assert.Equal("20.5", result.Station.Single().WeatherData.Single().TemperatureC);
        Assert.Equal("5.2", result.Station.Single().WeatherData.Single().WindSpeed);
    }

    [Fact]
    public async Task GetWeatherForecastAsync_WithStationIdAndPeriod_ReturnsStationData()
    {
        // Arrange
        var temperatureData = CreateStationData("Station1", "18.3");
        var windData = CreateStationData("Station1", "3.1");

        _mockSmhiProvider.Setup(x => x.GetStationTemperatureDataAsync("123", "latest-hour"))
            .ReturnsAsync(temperatureData);
        _mockSmhiProvider.Setup(x => x.GetStationWindDataAsync("123", "latest-hour"))
            .ReturnsAsync(windData);

        // Act
        var result = await _service.GetWeatherForecastAsync("123", Period.LatestHour);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Station);
        Assert.Equal("Station1", result.Station.Single().Name);
        Assert.Single(result.Station.Single().WeatherData);
        Assert.Equal("18.3", result.Station.Single().WeatherData.Single().TemperatureC);
        Assert.Equal("3.1", result.Station.Single().WeatherData.Single().WindSpeed);
    }

    [Theory]
    [InlineData(Period.LatestHour, "latest-hour")]
    [InlineData(Period.LatestDay, "latest-day")]
    public async Task GetWeatherForecastAsync_MapsPeriodsCorrectly(Period period, string expectedSmhiPeriod)
    {
        // Arrange
        var temperatureData = CreateStationData("Station1", "15.0");
        var windData = CreateStationData("Station1", "2.5");

        _mockSmhiProvider.Setup(x => x.GetStationTemperatureDataAsync("456", expectedSmhiPeriod))
            .ReturnsAsync(temperatureData);
        _mockSmhiProvider.Setup(x => x.GetStationWindDataAsync("456", expectedSmhiPeriod))
            .ReturnsAsync(windData);

        // Act
        await _service.GetWeatherForecastAsync("456", period);

        // Assert
        _mockSmhiProvider.Verify(x => x.GetStationTemperatureDataAsync("456", expectedSmhiPeriod), Times.Once);
        _mockSmhiProvider.Verify(x => x.GetStationWindDataAsync("456", expectedSmhiPeriod), Times.Once);
    }

    private static SmhiStationSetData CreateStationSetData(string stationName, string value, string stationKey)
    {
        return new SmhiStationSetData
        {
            Station = new List<SmhiStationSetDetails>
            {
                new()
                {
                    Name = stationName,
                    Key = stationKey,
                    Value = new List<SmhiValue>
                    {
                        new()
                        {
                            Date = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                            Value = value,
                            Quality = "G"
                        }
                    }
                }
            }
        };
    }

    private static SmhiStationData CreateStationData(string stationName, string value)
    {
        return new SmhiStationData
        {
            Station = new SmhiStationDetails
            {
                Name = stationName,
                Key = "test-key"
            },
            Value = new List<SmhiValue>
            {
                new()
                {
                    Date = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    Value = value,
                    Quality = "G"
                }
            }
        };
    }
}