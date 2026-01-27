using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Pogodnik.Data;
using Pogodnik.Data.Entities;
using Pogodnik.Data.Repositories;
using Pogodnik.Models.ApiModels;
using Pogodnik.Models.ViewModels;
using Pogodnik.Services.Interfaces;
using System.Text.Json;

namespace Pogodnik.Services
{
    public class WeatherApiSettings
    {
        public string ApiKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
        public string Units { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
    }

    public class OpenWeatherMapService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly WeatherApiSettings _settings;
        private readonly IMemoryCache _cache;
        private readonly ILogger<OpenWeatherMapService> _logger;
        private readonly ISearchHistoryRepository _searchHistoryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OpenWeatherMapService(
            HttpClient httpClient,
            IOptions<WeatherApiSettings> settings,
            IMemoryCache cache,
            ILogger<OpenWeatherMapService> logger,
            ISearchHistoryRepository searchHistoryRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _cache = cache;
            _logger = logger;
            _searchHistoryRepository = searchHistoryRepository;
            _httpContextAccessor = httpContextAccessor;

            if (_settings.BaseUrl != null)
                _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
        }

        public async Task<CurrentWeatherViewModel> GetWeatherAsync(string city)
        {
            var cacheKey = $"weather_{city.ToLower()}";

            if (_cache.TryGetValue(cacheKey, out CurrentWeatherViewModel? cachedWeather))
            {
                _logger.LogInformation($"Погода для города {city} получена из кэша");
                return cachedWeather!;
            }

            if (string.IsNullOrEmpty(_settings.ApiKey))
                throw new InvalidOperationException("API ключ не настроен");

            try
            {
                var url = $"weather?q={city}&appid={_settings.ApiKey}&units={_settings.Units}&lang={_settings.Language}";
                _logger.LogInformation($"Запрос погоды для города: {city}");

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var weatherData = JsonSerializer.Deserialize<OpenWeatherResponse>(json);

                if (weatherData == null)
                {
                    _logger.LogWarning($"Не удалось десериализовать данные для города: {city}");
                    throw new Exception("Не удалось получить данные о погоде");
                }

                var weatherViewModel = MapToViewModel(weatherData);

                // Сохраняем в кэш
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
                _cache.Set(cacheKey, weatherViewModel, cacheOptions);

                // Сохраняем в историю поиска
                await SaveToSearchHistory(city, weatherViewModel);

                return weatherViewModel;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"Ошибка HTTP при получении погоды для города: {city}");
                throw;
            }
        }

        private async Task SaveToSearchHistory(string city, CurrentWeatherViewModel weather)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null) return;

                var searchHistory = new SearchHistory
                {
                    CityName = weather.CityName,
                    CountryCode = weather.Country,
                    UserId = GetUserId(),
                    SearchedAt = DateTime.UtcNow,
                    Temperature = weather.Temperature,
                    WeatherDescription = weather.Description,
                    UserIP = httpContext.Connection.RemoteIpAddress?.ToString() ?? "",
                    UserAgent = httpContext.Request.Headers["User-Agent"].ToString()
                };

                await _searchHistoryRepository.AddSearchAsync(searchHistory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при сохранении истории поиска");
            }
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

        private CurrentWeatherViewModel MapToViewModel(OpenWeatherResponse data)
        {
            var weather = data.Weather?.FirstOrDefault();

            return new CurrentWeatherViewModel
            {
                CityName = data.Name ?? "Неизвестный город",
                Country = data.Sys?.Country ?? "",
                Temperature = data.Main?.Temp ?? 0,
                FeelsLike = data.Main?.FeelsLike ?? 0,
                Description = weather?.Description ?? "Нет данных",
                Icon = weather?.Icon ?? "01d",
                Humidity = data.Main?.Humidity ?? 0,
                Pressure = data.Main?.Pressure ?? 0,
                WindSpeed = data.Wind?.Speed ?? 0,
                WindDirection = data.Wind?.Deg ?? 0,
                Sunrise = data.Sys != null ?
                    DateTimeOffset.FromUnixTimeSeconds(data.Sys.Sunrise).LocalDateTime : DateTime.Now,
                Sunset = data.Sys != null ?
                    DateTimeOffset.FromUnixTimeSeconds(data.Sys.Sunset).LocalDateTime : DateTime.Now,
                LastUpdated = DateTime.Now
            };
        }

        public async Task<ForecastResponseViewModel> GetForecastAsync(string city)
        {
            var cacheKey = $"forecast_{city.ToLower()}";

            if (_cache.TryGetValue(cacheKey, out ForecastResponseViewModel? cachedForecast))
            {
                _logger.LogInformation($"Прогноз для города {city} получен из кэша");
                return cachedForecast!;
            }

            if (string.IsNullOrEmpty(_settings.ApiKey))
                throw new InvalidOperationException("API ключ не настроен");

            try
            {
                var url = $"forecast?q={city}&appid={_settings.ApiKey}&units={_settings.Units}&lang={_settings.Language}&cnt=40";
                _logger.LogInformation($"Запрос прогноза для города: {city}");

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                var forecastResponse = new ForecastResponseViewModel
                {
                    CityName = root.GetProperty("city").GetProperty("name").GetString() ?? city,
                    Country = root.GetProperty("city").GetProperty("country").GetString() ?? ""
                };

                var forecasts = new List<ForecastViewModel>();
                var dailyData = new Dictionary<DateTime, List<JsonElement>>();

                // Группируем по дням
                foreach (var item in root.GetProperty("list").EnumerateArray())
                {
                    var dt = DateTimeOffset.FromUnixTimeSeconds(item.GetProperty("dt").GetInt64()).Date;
                    if (!dailyData.ContainsKey(dt))
                        dailyData[dt] = new List<JsonElement>();
                    dailyData[dt].Add(item);
                }

                // Берем данные на ближайшие 7 дней
                var today = DateTime.Today;
                var culture = new System.Globalization.CultureInfo("ru-RU");

                foreach (var day in dailyData.OrderBy(x => x.Key).Take(7))
                {
                    // Для каждого дня берем данные на 12:00 или ближайшие
                    var noonItem = day.Value
                        .OrderBy(x => Math.Abs(
                            DateTimeOffset.FromUnixTimeSeconds(x.GetProperty("dt").GetInt64()).Hour - 12))
                        .FirstOrDefault();

                    if (noonItem.ValueKind == JsonValueKind.Undefined)
                        continue;

                    var main = noonItem.GetProperty("main");
                    var weather = noonItem.GetProperty("weather")[0];
                    var wind = noonItem.GetProperty("wind");

                    var date = day.Key;
                    var dayOfWeek = date.ToString("dddd", culture);
                    var dayOfWeekShort = date.ToString("ddd", culture);

                    // Для температуры мин/макс используем все данные за день
                    var dayTemps = day.Value.Select(x => x.GetProperty("main").GetProperty("temp").GetDouble());
                    var minTemp = dayTemps.Min();
                    var maxTemp = dayTemps.Max();

                    forecasts.Add(new ForecastViewModel
                    {
                        Date = date,
                        DayOfWeek = char.ToUpper(dayOfWeek[0]) + dayOfWeek.Substring(1),
                        DayOfWeekShort = dayOfWeekShort,
                        Temperature = main.GetProperty("temp").GetDouble(),
                        TemperatureMin = minTemp,
                        TemperatureMax = maxTemp,
                        Description = weather.GetProperty("description").GetString() ?? "",
                        Icon = weather.GetProperty("icon").GetString() ?? "01d",
                        Humidity = main.GetProperty("humidity").GetInt32(),
                        WindSpeed = wind.GetProperty("speed").GetDouble()
                    });
                }

                forecastResponse.Forecasts = forecasts;

                // Сохраняем в кэш
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));
                _cache.Set(cacheKey, forecastResponse, cacheOptions);

                return forecastResponse;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"Ошибка HTTP при получении прогноза для города: {city}");
                throw;
            }
        }
    }
}