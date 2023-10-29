using System.Collections.Generic;
using System.Threading.Tasks;
using Weather.Models;

namespace Weather.Services
{
    public interface IWeatherService
    {
        List<string> GetCountries();
        List<string> GetCities(string country);
        Task<WeatherViewModel> GetWeatherAsync(string city);
    }
}
