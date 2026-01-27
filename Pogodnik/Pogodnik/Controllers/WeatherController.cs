using Microsoft.AspNetCore.Mvc;
using Pogodnik.Services;
using Pogodnik.Services.Interfaces;

namespace Pogodnik.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IWeatherService _weatherService;
        private readonly IFavoritesService _favoritesService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WeatherController(
            IWeatherService weatherService,
            IFavoritesService favoritesService,
            IHttpContextAccessor httpContextAccessor)
        {
            _weatherService = weatherService;
            _favoritesService = favoritesService;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: /Weather или /Weather/Index
        public async Task<IActionResult> Index(string? city = null)
        {
            city ??= "Воронеж";

            try
            {
                var weather = await _weatherService.GetWeatherAsync(city);

                // Проверяем, есть ли город в избранном
                ViewBag.IsFavorite = await IsCityFavorite(weather.CityName);

                return View(weather);
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("404"))
            {
                ViewBag.Error = $"Город '{city}' не найден";
                return View();
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Error = $"Ошибка API: {ex.Message}";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Произошла ошибка: {ex.Message}";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetWeather(string? city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                ViewBag.Error = "Введите название города";
                return View("Index");
            }

            try
            {
                var weather = await _weatherService.GetWeatherAsync(city);

                // Проверяем, есть ли город в избранном
                ViewBag.IsFavorite = await IsCityFavorite(weather.CityName);

                return View("Index", weather);
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("404"))
            {
                ViewBag.Error = $"Город '{city}' не найден";
                return View("Index");
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Error = $"Ошибка API: {ex.Message}";
                return View("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Произошла ошибка: {ex.Message}";
                return View("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(string city, string country)
        {
            try
            {
                var userId = GetUserId();
                var sessionId = GetSessionId();

                var isFavorite = await _favoritesService.IsCityFavoriteAsync(city, userId);

                if (isFavorite)
                {
                    await _favoritesService.RemoveFavoriteAsync(city, userId);
                    return Json(new { success = true, isFavorite = false, message = "Город удален из избранного" });
                }
                else
                {
                    await _favoritesService.AddFavoriteAsync(city, country, userId, sessionId);
                    return Json(new { success = true, isFavorite = true, message = "Город добавлен в избранное" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private async Task<bool> IsCityFavorite(string cityName)
        {
            var userId = GetUserId();
            return await _favoritesService.IsCityFavoriteAsync(cityName, userId);
        }

        private string GetUserId()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.User?.Identity?.IsAuthenticated == true)
            {
                return httpContext.User.Identity.Name ?? "anonymous";
            }

            // Используем SessionId для анонимных пользователей
            return httpContext?.Session?.Id ?? "anonymous";
        }

        private string GetSessionId()
        {
            return _httpContextAccessor.HttpContext?.Session?.Id ?? "unknown";
        }
        [HttpGet]
        public async Task<IActionResult> CheckApiStatus()
        {
            try
            {
                // Простая проверка доступности API
                var testResponse = await _weatherService.GetWeatherAsync("London");
                return Ok();
            }
            catch
            {
                return StatusCode(503);
            }
        }
    }

}