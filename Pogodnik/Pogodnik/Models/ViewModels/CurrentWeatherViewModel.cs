using System;

namespace Pogodnik.Models.ViewModels
{
    public class CurrentWeatherViewModel
    {
        public string CityName { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public double FeelsLike { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public int Humidity { get; set; }
        public int Pressure { get; set; }
        public double WindSpeed { get; set; }
        public int WindDirection { get; set; }
        public DateTime Sunrise { get; set; }
        public DateTime Sunset { get; set; }
        public DateTime LastUpdated { get; set; }

        public string WindDirectionText
        {
            get
            {
                if (WindDirection >= 338 || WindDirection < 23) return "С";
                if (WindDirection >= 23 && WindDirection < 68) return "СВ";
                if (WindDirection >= 68 && WindDirection < 113) return "В";
                if (WindDirection >= 113 && WindDirection < 158) return "ЮВ";
                if (WindDirection >= 158 && WindDirection < 203) return "Ю";
                if (WindDirection >= 203 && WindDirection < 248) return "ЮЗ";
                if (WindDirection >= 248 && WindDirection < 293) return "З";
                return "СЗ";
            }
        }
    }
}