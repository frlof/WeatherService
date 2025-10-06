# WeatherService

## Testing TODO

- [ ] **Multiple Station Testing**: Add tests for multiple stations with no parameters
- [ ] **Test Data Builder**: Create `SmhiStationSetDataBuilder` with functions like `.AddStation()`
- [ ] **Edge Cases**: Test empty data, missing temperature/wind data
- [ ] **Separate Test Classes**: Split by functionality (station set, single station, edge cases)

## Features TODO

- [ ] **Database Caching**: Add database to cache data fetched from SMHI
- [ ] **Redis Cache**: Add Redis for high-performance caching to improve response times