using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Weather.Helpers;
using Weather.Models;

namespace Weather.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly ILogger<WeatherService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;
        public WeatherService(ILogger<WeatherService> logger, IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _config = config;
        }
        public List<string> GetCountries()
        {
            try
            {
                return CountriesHelper.GetCountries();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }

        public List<string> GetCities(string country)
        {
            try
            {
                return CountriesHelper.GetCities(country);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }

        public async Task<WeatherViewModel> GetWeatherAsync(string city)
        {
            try
            {
                var apiKey = _config["WeatherAPIKey"];
                var httpClient = _httpClientFactory.CreateClient();

                var response = await httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    var weatherData = JsonConvert.DeserializeObject<WeatherData>(content);

                    var tempC = weatherData.main.temp - 273.15;
                    var humidity = weatherData.main.humidity;
                    var dewPoint = GetDewPoint(tempC, humidity);

                    var result = new WeatherViewModel()
                    {
                        Location = weatherData.name,
                        Time = DateTimeOffset.FromUnixTimeSeconds(weatherData.dt).DateTime,
                        Wind = $"{weatherData.wind.speed} / {weatherData.wind.deg}°",
                        Visibility = weatherData.visibility,
                        SkyCondition = weatherData.weather.First().main,
                        TempInC = tempC.ToString("0.00"),
                        TempInF = (weatherData.main.temp * 9 / 5 - 459.67).ToString("0.00"),
                        DewPoint = dewPoint.ToString("0.00"),
                        RelativeHumidity = humidity,
                        Pressure = weatherData.main.pressure
                    };

                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }

        private double GetDewPoint(double tempC, double humidity)
        {
            double a = 17.271;
            double b = 237.7;

            double gamma = Math.Log(humidity / 100) + (a * tempC) / (b + tempC);
            double dewPointC = (b * gamma) / (a - gamma);
            return dewPointC;
        }
    }
}
