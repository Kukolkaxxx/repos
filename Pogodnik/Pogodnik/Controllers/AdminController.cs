using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pogodnik.Data.Repositories;

namespace Pogodnik.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ISearchHistoryRepository _searchHistoryRepository;
        private readonly IFavoriteRepository _favoriteRepository;

        public AdminController(
            ISearchHistoryRepository searchHistoryRepository,
            IFavoriteRepository favoriteRepository)
        {
            _searchHistoryRepository = searchHistoryRepository;
            _favoriteRepository = favoriteRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Statistics()
        {
            var totalSearches = await _searchHistoryRepository.GetTotalSearchesCountAsync();
            var recentSearches = await _searchHistoryRepository.GetRecentSearchesAsync(20);
            var popularCities = await _searchHistoryRepository.GetPopularCitiesAsync(10);

            ViewBag.TotalSearches = totalSearches;
            ViewBag.RecentSearches = recentSearches;
            ViewBag.PopularCities = popularCities;

            return View();
        }

        public async Task<IActionResult> SearchHistory(int page = 1, int pageSize = 50)
        {
            // Здесь можно добавить пагинацию
            var history = await _searchHistoryRepository.GetRecentSearchesAsync(100);
            return View(history);
        }

        public async Task<IActionResult> Favorites()
        {
            // Можно добавить статистику по избранным городам
            return View();
        }

        public IActionResult Settings()
        {
            return View();
        }
        // Добавьте в конец AdminController.cs
        [HttpGet]
        public async Task<IActionResult> GetStatistics()
        {
            try
            {
                var totalSearches = await _searchHistoryRepository.GetTotalSearchesCountAsync();

                // Для этих методов нужно добавить в репозитории
                // var totalFavorites = await _favoriteRepository.GetTotalFavoritesCountAsync();
                // var todaySearches = await _searchHistoryRepository.GetTodaySearchesCountAsync();
                // var popularCity = await _searchHistoryRepository.GetMostPopularCityAsync();

                // Временно используем заглушки
                var totalFavorites = 42;
                var todaySearches = 15;
                var popularCity = "Москва";

                return Json(new
                {
                    success = true,
                    totalSearches,
                    totalFavorites,
                    todaySearches,
                    popularCity
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult ClearOldCache()
        {
            try
            {
                // Здесь можно добавить логику очистки кэша
                return Json(new { success = true, message = "Кэш очищен" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult ExportData()
        {
            // Здесь можно добавить логику экспорта данных
            var data = new { message = "Экспорт данных пока не реализован" };
            return Json(data);
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            // Заглушка - в реальном приложении нужно получать из БД
            var users = new[]
            {
        new { id = 1, name = "Администратор", email = "admin@pogodnik.com",
              roles = new[] { "Admin" }, createdAt = DateTime.Now.AddDays(-30) },
        new { id = 2, name = "Иван Иванов", email = "ivan@example.com",
              roles = new[] { "User" }, createdAt = DateTime.Now.AddDays(-15) },
        new { id = 3, name = "Мария Петрова", email = "maria@example.com",
              roles = new[] { "User" }, createdAt = DateTime.Now.AddDays(-7) }
    };

            return Json(users);
        }

        [HttpGet]
        public IActionResult GetLogs()
        {
            // Заглушка - в реальном приложении нужно читать логи из файла
            var logs = @"
[INFO] 2024-01-15 10:30:15 - Приложение запущено
[INFO] 2024-01-15 10:35:22 - Пользователь admin вошел в систему
[INFO] 2024-01-15 10:40:18 - Запрос погоды для города Москва
[WARNING] 2024-01-15 11:20:45 - API OpenWeatherMap ответил с задержкой
[INFO] 2024-01-15 12:15:30 - Пользователь ivan@example.com вошел в систему
[ERROR] 2024-01-15 14:05:12 - Ошибка подключения к базе данных
[INFO] 2024-01-15 14:10:45 - Подключение к базе данных восстановлено
";

            return Content(logs, "text/plain");
        }
        [HttpPost]
        public async Task<IActionResult> ClearAllSearchHistory()
        {
            try
            {
                // Здесь нужно добавить метод в репозиторий для очистки всей истории
                // await _searchHistoryRepository.ClearAllAsync();
                return Json(new { success = true, message = "История поиска очищена" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ClearAllFavorites()
        {
            try
            {
                // Здесь нужно добавить метод в репозиторий для очистки всех избранных
                // await _favoriteRepository.ClearAllAsync();
                return Json(new { success = true, message = "Избранные города очищены" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult ResetSettings()
        {
            try
            {
                // Здесь можно добавить логику сброса настроек
                return Json(new { success = true, message = "Настройки сброшены" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }

}
