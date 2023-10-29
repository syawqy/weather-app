using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Weather.Helpers
{
    public static class CountriesHelper
    {
        private static Dictionary<string, List<string>> countryAndCities = new Dictionary<string, List<string>>() {
            {"canada", new List<string>() {"Toronto", "Montreal", "Vancouver"}},
            {"united states", new List<string>() {"New York", "Los Angeles", "Chicago"}},
            {"australia", new List<string>() {"Sydney", "Melbourne", "Perth"}},
            {"indonesia", new List<string>() {"Jakarta", "Depok", "Bekasi", "Bogor"}}
        };

        public static List<string> GetCountries()
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            return countryAndCities.Keys.Select(a => textInfo.ToTitleCase(a)).ToList();
        }
        public static List<string> GetCities(string countryName)
        {
            return countryAndCities.TryGetValue(countryName.ToLower(), out List<string> cities) ? cities : new List<string>();
        }
    }
}
