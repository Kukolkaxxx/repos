using Pogodnik.Data.Entities;
using Pogodnik.Data.Repositories;
using Pogodnik.Services.Interfaces;

namespace Pogodnik.Services
{
    public interface IFavoritesService
    {
        Task<IEnumerable<FavoriteCity>> GetUserFavoritesAsync(string userId);
        Task AddFavoriteAsync(string cityName, string countryCode, string userId, string userSessionId);
        Task RemoveFavoriteAsync(string cityName, string userId);
        Task<bool> IsCityFavoriteAsync(string cityName, string userId);
        Task<int> GetFavoritesCountAsync(string userId);
    }

    public class FavoritesService : IFavoritesService
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public FavoritesService(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public async Task<IEnumerable<FavoriteCity>> GetUserFavoritesAsync(string userId)
        {
            return await _favoriteRepository.GetUserFavoritesAsync(userId);
        }

        public async Task AddFavoriteAsync(string cityName, string countryCode, string userId, string userSessionId)
        {
            var favorite = new FavoriteCity
            {
                CityName = cityName,
                CountryCode = countryCode,
                UserId = userId,
                UserSessionId = userSessionId,
                AddedAt = DateTime.UtcNow
            };

            await _favoriteRepository.AddFavoriteAsync(favorite);
        }

        public async Task RemoveFavoriteAsync(string cityName, string userId)
        {
            await _favoriteRepository.RemoveFavoriteByCityAsync(cityName, userId);
        }

        public async Task<bool> IsCityFavoriteAsync(string cityName, string userId)
        {
            return await _favoriteRepository.IsCityFavoriteAsync(cityName, userId);
        }

        public async Task<int> GetFavoritesCountAsync(string userId)
        {
            return await _favoriteRepository.GetUserFavoritesCountAsync(userId);
        }
    }
}