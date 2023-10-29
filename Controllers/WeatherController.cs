using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Weather.Services;

namespace Weather.Controllers
{
    [Route("api/[controller]")]
    public class WeatherController : Controller
    {
        private readonly IWeatherService _weatherService;
        private readonly ILogger<WeatherController> _logger;
        public WeatherController(IWeatherService weatherServices, ILogger<WeatherController> logger) 
        {
            _weatherService = weatherServices;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Weather([FromQuery] string city)
        {
            try
            {
                var weather = await _weatherService.GetWeatherAsync(city);

                return Ok(weather);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("countries")]
        public IActionResult Countries()
        {
            try
            {
                var countries = _weatherService.GetCountries();

                return Ok(countries);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("cities/{country}")]
        public IActionResult Cities(string country)
        {
            try
            {
                var cities = _weatherService.GetCities(country);

                return Ok(cities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }
    }
}
