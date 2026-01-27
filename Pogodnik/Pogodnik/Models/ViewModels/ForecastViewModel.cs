using System;

namespace Pogodnik.Models.ViewModels
{
    public class ForecastViewModel
    {
        public DateTime Date { get; set; }
        public string DayOfWeek { get; set; } = string.Empty;
        public string DayOfWeekShort { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public double TemperatureMin { get; set; }
        public double TemperatureMax { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }

        public string FormattedDate => Date.ToString("dd.MM");
    }

    public class ForecastResponseViewModel
    {
        public string CityName { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public List<ForecastViewModel> Forecasts { get; set; } = new List<ForecastViewModel>();
    }
}