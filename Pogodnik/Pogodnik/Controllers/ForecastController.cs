using Microsoft.AspNetCore.Mvc;
using Pogodnik.Services.Interfaces;

namespace Pogodnik.Controllers
{
    public class ForecastController : Controller
    {
        private readonly IWeatherService _weatherService;

        public ForecastController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        // GET: /Forecast или /Forecast/Index
        public async Task<IActionResult> Index(string? city = null)
        {
            try
            {
                city ??= "Воронеж";
                var forecast = await _weatherService.GetForecastAsync(city);
                return View(forecast);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Ошибка: {ex.Message}";
                var testData = CreateTestData(city ?? "Воронеж");
                return View(testData);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetForecast(string? city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                ViewBag.Error = "Введите название города";
                return View("Index", CreateTestData("Воронеж"));
            }

            try
            {
                var forecast = await _weatherService.GetForecastAsync(city);
                return View("Index", forecast);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Ошибка: {ex.Message}";
                var testData = CreateTestData(city);
                return View("Index", testData);
            }
        }

        private Pogodnik.Models.ViewModels.ForecastResponseViewModel CreateTestData(string city)
        {
            var forecastResponse = new Pogodnik.Models.ViewModels.ForecastResponseViewModel
            {
                CityName = city,
                Country = "RU",
                Forecasts = new List<Pogodnik.Models.ViewModels.ForecastViewModel>()
            };

            var culture = new System.Globalization.CultureInfo("ru-RU");

            for (int i = 0; i < 7; i++)
            {
                var date = DateTime.Now.AddDays(i);
                var dayOfWeek = date.ToString("dddd", culture);
                var dayOfWeekShort = date.ToString("ddd", culture);

                forecastResponse.Forecasts.Add(new Pogodnik.Models.ViewModels.ForecastViewModel
                {
                    Date = date,
                    DayOfWeek = char.ToUpper(dayOfWeek[0]) + dayOfWeek.Substring(1),
                    DayOfWeekShort = dayOfWeekShort,
                    Temperature = 15 + i * 2,
                    TemperatureMin = 10 + i,
                    TemperatureMax = 20 + i,
                    Description = i % 2 == 0 ? "Солнечно" : "Облачно",
                    Icon = i % 2 == 0 ? "01d" : "02d",
                    Humidity = 60 - i * 5,
                    WindSpeed = 3 + i
                });
            }

            return forecastResponse;
        }
    }
}