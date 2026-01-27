using System;
using System.ComponentModel.DataAnnotations;

namespace Pogodnik.Data.Entities
{
    public class WeatherCache
    {
        [Key]
        [MaxLength(100)]
        public string CityName { get; set; } = string.Empty;

        [Required]
        public string WeatherData { get; set; } = string.Empty; // JSON данные

        public DateTime CachedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        [MaxLength(10)]
        public string CountryCode { get; set; } = string.Empty;
    }
}