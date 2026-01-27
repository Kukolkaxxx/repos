using Microsoft.AspNetCore.Mvc;

namespace Pogodnik.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Перенаправляем на страницу погоды
            return RedirectToAction("Index", "Weather");
        }

        public IActionResult Privacy()
        {
            ViewData["Title"] = "О сервисе";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}