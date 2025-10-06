# WeatherService

## Testing TODO

- [ ] **Multiple Station Testing**: Add tests for multiple stations with no parameters
- [ ] **Test Data Builder**: Create `SmhiStationSetDataBuilder` with functions like `.AddStation()`
- [ ] **Edge Cases**: Test empty data, missing temperature/wind data

## Features TODO

- [ ] **In-Memory Caching**: Add in-memory caching for improved performance
- [ ] **Database Caching**: Add database to cache data fetched from SMHI
- [ ] **Redis Cache**: Add Redis for high-performance caching to improve response times

## Security TODO

- [ ] **API Key Management**: Replace simple API key validation with Microsoft Entra ID authentication
- [ ] **Security Middleware**: Move authentication from endpoint-specific to middleware applying to all endpoints