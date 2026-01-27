using System;
using System.ComponentModel.DataAnnotations;

namespace Pogodnik.Data.Entities
{
    public class SearchHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string CityName { get; set; } = string.Empty;

        [MaxLength(10)]
        public string CountryCode { get; set; } = string.Empty;

        [MaxLength(200)]
        public string UserId { get; set; } = string.Empty;

        public DateTime SearchedAt { get; set; }

        public double? Temperature { get; set; }

        [MaxLength(50)]
        public string WeatherDescription { get; set; } = string.Empty;

        [MaxLength(200)]
        public string UserIP { get; set; } = string.Empty;

        [MaxLength(500)]
        public string UserAgent { get; set; } = string.Empty;
    }
}