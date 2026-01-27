using Pogodnik.Data.Entities;

namespace Pogodnik.Data.Repositories
{
    public interface IFavoriteRepository
    {
        Task<IEnumerable<FavoriteCity>> GetUserFavoritesAsync(string userId);
        Task<FavoriteCity?> GetFavoriteAsync(int id);
        Task<FavoriteCity?> GetFavoriteByCityAsync(string cityName, string userId);
        Task AddFavoriteAsync(FavoriteCity favorite);
        Task RemoveFavoriteAsync(int id);
        Task RemoveFavoriteByCityAsync(string cityName, string userId);
        Task<bool> IsCityFavoriteAsync(string cityName, string userId);
        Task<int> GetUserFavoritesCountAsync(string userId);
    }
}