using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Pogodnik.Pages.Admin
{
    public class SettingsModel : PageModel
    {
        [BindProperty]
        public ApiSettings ApiSettings { get; set; } = new ApiSettings();

        [BindProperty]
        public SystemSettings SystemSettings { get; set; } = new SystemSettings();

        public void OnGet()
        {
            // Загружаем настройки из базы данных или конфигурации
            // Для примера устанавливаем значения по умолчанию
            ApiSettings = new ApiSettings
            {
                Units = "metric",
                Language = "ru",
                EnableCaching = true,
                CacheDuration = 10
            };

            SystemSettings = new SystemSettings
            {
                SiteName = "Pogodnik",
                SiteDescription = "Современный погодный сервис",
                DefaultCity = "Воронеж",
                SearchHistoryLimit = 50,
                MaxFavorites = 20,
                EnableRegistration = true,
                EnableNotifications = false
            };

            ViewData["Title"] = "Настройки";
        }

        public IActionResult OnPostSaveApiSettings()
        {
            if (!ModelState.IsValid)
            {
                // Переинициализируем ViewData при возврате страницы с ошибками
                ViewData["Title"] = "Настройки";
                return Page();
            }

            try
            {
                // Сохраняем настройки API в базу данных или конфигурацию
                TempData["SuccessMessage"] = "Настройки API успешно сохранены!";
                ViewData["Title"] = "Настройки";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ошибка при сохранении настроек: {ex.Message}");
                ViewData["Title"] = "Настройки";
                return Page();
            }
        }

        public IActionResult OnPostSaveSystemSettings()
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Настройки";
                return Page();
            }

            try
            {
                // Сохраняем системные настройки
                TempData["SuccessMessage"] = "Системные настройки успешно сохранены!";
                ViewData["Title"] = "Настройки";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ошибка при сохранении настроек: {ex.Message}");
                ViewData["Title"] = "Настройки";
                return Page();
            }
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostClearAllSearchHistory()
        {
            try
            {
                // Очищаем историю поиска
                TempData["SuccessMessage"] = "История поиска очищена!";
                ViewData["Title"] = "Настройки";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ошибка: {ex.Message}";
                ViewData["Title"] = "Настройки";
                return RedirectToPage();
            }
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostClearAllFavorites()
        {
            try
            {
                // Очищаем все избранные города
                TempData["SuccessMessage"] = "Все избранные города удалены!";
                ViewData["Title"] = "Настройки";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ошибка: {ex.Message}";
                ViewData["Title"] = "Настройки";
                return RedirectToPage();
            }
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostResetSettings()
        {
            try
            {
                // Сбрасываем настройки к значениям по умолчанию
                TempData["SuccessMessage"] = "Настройки сброшены к значениям по умолчанию!";
                ViewData["Title"] = "Настройки";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ошибка: {ex.Message}";
                ViewData["Title"] = "Настройки";
                return RedirectToPage();
            }
        }
    }

    public class ApiSettings
    {
        [Display(Name = "API Key")]
        public string? ApiKey { get; set; }

        [Required(ErrorMessage = "Выберите единицы измерения")]
        [Display(Name = "Единицы измерения")]
        public string Units { get; set; } = "metric";

        [Required(ErrorMessage = "Выберите язык")]
        [Display(Name = "Язык")]
        public string Language { get; set; } = "ru";

        [Display(Name = "Включить кэширование")]
        public bool EnableCaching { get; set; } = true;

        [Range(1, 1440, ErrorMessage = "Значение должно быть от 1 до 1440")]
        [Display(Name = "Время жизни кэша (минуты)")]
        public int CacheDuration { get; set; } = 10;
    }

    public class SystemSettings
    {
        [Required(ErrorMessage = "Введите название сайта")]
        [StringLength(50, ErrorMessage = "Название не должно превышать 50 символов")]
        [Display(Name = "Название сайта")]
        public string SiteName { get; set; } = "Pogodnik";

        [StringLength(200, ErrorMessage = "Описание не должно превышать 200 символов")]
        [Display(Name = "Описание сайта")]
        public string SiteDescription { get; set; } = "Современный погодный сервис";

        [Required(ErrorMessage = "Введите город по умолчанию")]
        [Display(Name = "Город по умолчанию")]
        public string DefaultCity { get; set; } = "Воронеж";

        [Range(10, 1000, ErrorMessage = "Значение должно быть от 10 до 1000")]
        [Display(Name = "Лимит истории поиска")]
        public int SearchHistoryLimit { get; set; } = 50;

        [Range(5, 100, ErrorMessage = "Значение должно быть от 5 до 100")]
        [Display(Name = "Максимальное количество избранных")]
        public int MaxFavorites { get; set; } = 20;

        [Display(Name = "Разрешить регистрацию")]
        public bool EnableRegistration { get; set; } = true;

        [Display(Name = "Включить уведомления")]
        public bool EnableNotifications { get; set; } = false;
    }
}