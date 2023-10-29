using System;

namespace Weather.Models
{
    public class WeatherViewModel
    {
        public string Location { get; set; }
        public DateTime Time { get; set; }
        public string Wind { get; set; }
        public int Visibility { get; set; }
        public string SkyCondition { get; set; }
        public string TempInC { get; set; }
        public string TempInF { get; set; }
        public string DewPoint { get; set; }
        public int RelativeHumidity { get; set; }
        public int Pressure { get; set; }

    }
}
