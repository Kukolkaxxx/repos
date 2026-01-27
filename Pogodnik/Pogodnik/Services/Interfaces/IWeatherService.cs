using Pogodnik.Models.ViewModels;

namespace Pogodnik.Services.Interfaces
{
    public interface IWeatherService
    {
        Task<CurrentWeatherViewModel> GetWeatherAsync(string city);
        Task<ForecastResponseViewModel> GetForecastAsync(string city);
    }
}