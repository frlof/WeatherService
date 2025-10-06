using WeatherService.Interfaces.Commands;
using WeatherService.Service.Commands;
using WeatherService.Service.External.Smhi;
using WeatherService.Service.External.Smhi.Provider;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<SmhiApiOptions>(
    builder.Configuration.GetSection(SmhiApiOptions.SectionName));

builder.Services.AddHttpClient<ISmhiProvider, SmhiProvider>();
builder.Services.AddScoped<IWeatherForecast, GetWeatherForecast>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
