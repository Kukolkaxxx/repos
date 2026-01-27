using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pogodnik.Data.Entities;

namespace Pogodnik.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<FavoriteCity> FavoriteCities { get; set; }
        public DbSet<SearchHistory> SearchHistory { get; set; }
        public DbSet<WeatherCache> WeatherCache { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфигурация FavoriteCity
            modelBuilder.Entity<FavoriteCity>(entity =>
            {
                entity.HasIndex(e => new { e.CityName, e.UserSessionId }).IsUnique();
                entity.HasIndex(e => e.UserSessionId);
                entity.HasIndex(e => e.AddedAt);

                entity.Property(e => e.CityName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CountryCode).HasMaxLength(10);
                entity.Property(e => e.UserSessionId).HasMaxLength(50);
            });

            // Конфигурация SearchHistory
            modelBuilder.Entity<SearchHistory>(entity =>
            {
                entity.HasIndex(e => e.SearchedAt);
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.CityName);

                entity.Property(e => e.CityName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CountryCode).HasMaxLength(10);
                entity.Property(e => e.WeatherDescription).HasMaxLength(50);
                entity.Property(e => e.UserIP).HasMaxLength(200);
                entity.Property(e => e.UserAgent).HasMaxLength(500);
            });

            // Конфигурация WeatherCache
            modelBuilder.Entity<WeatherCache>(entity =>
            {
                entity.HasKey(e => e.CityName);
                entity.HasIndex(e => e.ExpiresAt);

                entity.Property(e => e.CityName).HasMaxLength(100);
                entity.Property(e => e.CountryCode).HasMaxLength(10);
                entity.Property(e => e.WeatherData).IsRequired();
            });

            // Конфигурация AppUser
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.Property(e => e.DefaultCity).HasMaxLength(100);
            });
        }
    }
}