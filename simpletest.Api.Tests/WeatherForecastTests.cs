using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace simpletest.Api.Tests;

public class WeatherForecastTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetWeatherForecast_ReturnsOk()
    {
        var response = await _client.GetAsync("/weatherforecast");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetWeatherForecast_ReturnsFiveItems()
    {
        var forecasts = await _client.GetFromJsonAsync<WeatherForecastDto[]>("/weatherforecast");

        Assert.NotNull(forecasts);
        Assert.Equal(5, forecasts.Length);
    }

    [Fact]
    public async Task GetWeatherForecast_EachItemHasValidTemperatureF()
    {
        var forecasts = await _client.GetFromJsonAsync<WeatherForecastDto[]>("/weatherforecast");

        Assert.NotNull(forecasts);
        foreach (var forecast in forecasts)
        {
            int expectedF = 32 + (int)(forecast.TemperatureC / 0.5556);
            Assert.Equal(expectedF, forecast.TemperatureF);
        }
    }

    [Fact]
    public async Task GetWeatherForecast_EachItemHasUniqueId()
    {
        var forecasts = await _client.GetFromJsonAsync<WeatherForecastDto[]>("/weatherforecast");

        Assert.NotNull(forecasts);
        var ids = forecasts.Select(f => f.Id).ToList();
        Assert.Equal(ids.Count, ids.Distinct().Count());
    }

    [Fact]
    public async Task GetWeatherForecast_ReturnsJsonContentType()
    {
        var response = await _client.GetAsync("/weatherforecast");

        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
    }

    private record WeatherForecastDto(Guid Id, DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
