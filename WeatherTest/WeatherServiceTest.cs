using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Weather.Helpers;
using Weather.Services;
using Xunit;

namespace WeatherTest
{
    public class WeatherServiceTest
    {
        private readonly Mock<ILogger<WeatherService>> _loggerMock = new Mock<ILogger<WeatherService>>();
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        private readonly Mock<IConfiguration> _configMock = new Mock<IConfiguration>();

        [Fact]
        public void GetCountries_Should_Return_CountriesList()
        {
            // Arrange
            var service = new WeatherService(_loggerMock.Object, _httpClientFactoryMock.Object, _configMock.Object);

            // Act
            var countries = service.GetCountries();

            // Assert
            Assert.NotNull(countries);
        }

        [Fact]
        public void GetCities_With_ValidCountry_Should_Return_CitiesList()
        {
            // Arrange
            var service = new WeatherService(_loggerMock.Object, _httpClientFactoryMock.Object, _configMock.Object);
            string country = "Indonesia";

            // Act
            var cities = service.GetCities(country);

            // Assert
            var expect = CountriesHelper.GetCities(country);
            Assert.Equal(expect, cities);
        }

        [Fact]
        public async Task GetWeatherAsync_With_ValidCity_Should_Return_WeatherViewModel()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            var dummyResponse = @"{""coord"":{""lon"":-0.1257,""lat"":51.5085},""weather"":[{""id"":804,""main"":""Clouds"",""description"":""overcast clouds"",""icon"":""04n""}],""base"":""stations"",""main"":{""temp"":283.43,""feels_like"":282.94,""temp_min"":282.11,""temp_max"":284.97,""pressure"":991,""humidity"":93},""visibility"":10000,""wind"":{""speed"":4.12,""deg"":200},""clouds"":{""all"":96},""dt"":1698601200,""sys"":{""type"":2,""id"":2006068,""country"":""GB"",""sunrise"":1698562071,""sunset"":1698597626},""timezone"":0,""id"":2643743,""name"":""London"",""cod"":200}";

            mockHttp.When("*").Respond("application/json", dummyResponse);
            var client = new HttpClient(mockHttp);

            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>()))
                .Returns(client).Verifiable();

            var service = new WeatherService(_loggerMock.Object, _httpClientFactoryMock.Object, _configMock.Object);

            // Act
            var weather = await service.GetWeatherAsync("London");

            // Assert
            Assert.NotNull(weather);
            Assert.Equal("Clouds", weather.SkyCondition);
            Assert.Equal((283.43- 273.15).ToString("0.00"), weather.TempInC);
        }

        [Fact]
        public async Task GetWeatherAsync_With_ErrorInApiCall_Should_Return_Null()
        {
            // Arrange
            var city = "Jakarta";
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When("*").Throw( new Exception("Failed to fetch data from API"));
            var client = new HttpClient(mockHttp);

            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>()))
                .Returns(client).Verifiable();

            var configMock = new Mock<IConfiguration>();

            var loggerMock = new Mock<ILogger<WeatherService>>();

            var weatherService = new WeatherService(loggerMock.Object, _httpClientFactoryMock.Object, configMock.Object);

            // Act
            var result = await weatherService.GetWeatherAsync(city);

            // Assert
            Assert.Null(result);
        }
    }
}
