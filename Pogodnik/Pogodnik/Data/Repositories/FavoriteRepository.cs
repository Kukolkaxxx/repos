using Microsoft.EntityFrameworkCore;
using Pogodnik.Data.Entities;

namespace Pogodnik.Data.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly ApplicationDbContext _context;

        public FavoriteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FavoriteCity>> GetUserFavoritesAsync(string userId)
        {
            return await _context.FavoriteCities
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.AddedAt)
                .ToListAsync();
        }

        public async Task<FavoriteCity?> GetFavoriteAsync(int id)
        {
            return await _context.FavoriteCities.FindAsync(id);
        }

        public async Task<FavoriteCity?> GetFavoriteByCityAsync(string cityName, string userId)
        {
            return await _context.FavoriteCities
                .FirstOrDefaultAsync(f => f.CityName == cityName && f.UserId == userId);
        }

        public async Task AddFavoriteAsync(FavoriteCity favorite)
        {
            await _context.FavoriteCities.AddAsync(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFavoriteAsync(int id)
        {
            var favorite = await GetFavoriteAsync(id);
            if (favorite != null)
            {
                _context.FavoriteCities.Remove(favorite);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveFavoriteByCityAsync(string cityName, string userId)
        {
            var favorite = await GetFavoriteByCityAsync(cityName, userId);
            if (favorite != null)
            {
                _context.FavoriteCities.Remove(favorite);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsCityFavoriteAsync(string cityName, string userId)
        {
            return await _context.FavoriteCities
                .AnyAsync(f => f.CityName == cityName && f.UserId == userId);
        }

        public async Task<int> GetUserFavoritesCountAsync(string userId)
        {
            return await _context.FavoriteCities
                .CountAsync(f => f.UserId == userId);
        }
    }
}