using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Weather.Controllers;
using Weather.Models;
using Weather.Services;
using Xunit;

namespace WeatherTest
{
    public class WeatherControllerTest
    {
        [Fact]
        public async Task Weather_With_ValidCity_Should_Return_OkResult()
        {
            // Arrange
            var city = "Jakarta";
            var expectedWeather = new WeatherViewModel()
            {
                Location = "Jakarta",
                Time = DateTime.UtcNow,
                Wind = "20.5 / 120°",
                Visibility = 1200,
                SkyCondition = "Clear",
                TempInC = "29.01",
                TempInF = "83",
                DewPoint = "45.2",
                RelativeHumidity = 56,
                Pressure = 180
            };

            var weatherServiceMock = new Mock<IWeatherService>();
            weatherServiceMock.Setup(x => x.GetWeatherAsync(city)).ReturnsAsync(expectedWeather);

            var loggerMock = new Mock<ILogger<WeatherController>>();

            var weatherController = new WeatherController(weatherServiceMock.Object, loggerMock.Object);

            // Act
            var result = await weatherController.Weather(city);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var weather = Assert.IsType<WeatherViewModel>(okResult.Value);
            Assert.Equal(expectedWeather, weather);
        }

        [Fact]
        public async Task Weather_With_ValidCity_Should_Return_BadRequestResult()
        {
            // Arrange
            var city = "Jakarta";

            var weatherServiceMock = new Mock<IWeatherService>();

            var loggerMock = new Mock<ILogger<WeatherController>>();

            var weatherController = new WeatherController(weatherServiceMock.Object, loggerMock.Object);

            var error = "Service unavailable";
            weatherServiceMock.Setup(x => x.GetWeatherAsync(city)).ThrowsAsync(new Exception(error));

            // Act
            var result = await weatherController.Weather(city);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(error, badRequestResult.Value);
        }

        [Fact]
        public void Countries_Should_Return_OkResult()
        {
            // Arrange
            var expectedCountries = new List<string> { "Australia", "Indonesia", "United States", "Canada" };

            var weatherServiceMock = new Mock<IWeatherService>();
            weatherServiceMock.Setup(x => x.GetCountries()).Returns(expectedCountries);

            var loggerMock = new Mock<ILogger<WeatherController>>();

            var weatherController = new WeatherController(weatherServiceMock.Object, loggerMock.Object);

            // Act
            var result = weatherController.Countries();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var countries = Assert.IsType<List<string>>(okResult.Value);
            Assert.Equal(expectedCountries, countries);
        }

        [Fact]
        public void Cities_With_ValidCountry_Should_Return_OkResult()
        {
            // Arrange
            var country = "Indonesia";
            var expectedCities = new List<string> { "Jakarta", "Depok", "Bekasi", "Bogor" };

            var weatherServiceMock = new Mock<IWeatherService>();
            weatherServiceMock.Setup(x => x.GetCities(country)).Returns(expectedCities);

            var loggerMock = new Mock<ILogger<WeatherController>>();

            var weatherController = new WeatherController(weatherServiceMock.Object, loggerMock.Object);

            // Act
            var result = weatherController.Cities(country);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var cities = Assert.IsType<List<string>>(okResult.Value);
            Assert.Equal(expectedCities, cities);
        }
    }
}
