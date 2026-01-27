using Pogodnik.Models.ViewModels;
using Pogodnik.Services.Interfaces;

namespace Pogodnik.Services
{
    public class TestWeatherService : IWeatherService
    {
        public Task<CurrentWeatherViewModel> GetWeatherAsync(string city)
        {
            return Task.FromResult(new CurrentWeatherViewModel
            {
                CityName = city,
                Country = "RU",
                Temperature = 25,
                Description = "Солнечно",
                Icon = "01d"
            });
        }

        public Task<ForecastResponseViewModel> GetForecastAsync(string city)
        {
            var forecast = new ForecastResponseViewModel
            {
                CityName = city,
                Country = "RU",
                Forecasts = new List<ForecastViewModel>()
            };

            for (int i = 0; i < 7; i++)
            {
                forecast.Forecasts.Add(new ForecastViewModel
                {
                    Date = DateTime.Now.AddDays(i),
                    DayOfWeek = DateTime.Now.AddDays(i).DayOfWeek.ToString(),
                    DayOfWeekShort = DateTime.Now.AddDays(i).ToString("ddd"),
                    Temperature = 20 + i,
                    Description = "Солнечно",
                    Icon = "01d"
                });
            }

            return Task.FromResult(forecast);
        }
    }
}