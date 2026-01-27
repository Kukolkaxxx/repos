using System;
using System.ComponentModel.DataAnnotations;

namespace Pogodnik.Data.Entities
{
    public class FavoriteCity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string CityName { get; set; } = string.Empty;

        [MaxLength(10)]
        public string CountryCode { get; set; } = string.Empty;

        [MaxLength(200)]
        public string UserId { get; set; } = string.Empty; // Пока будем использовать IP или SessionId

        public DateTime AddedAt { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        [MaxLength(50)]
        public string UserSessionId { get; set; } = string.Empty;
    }
}