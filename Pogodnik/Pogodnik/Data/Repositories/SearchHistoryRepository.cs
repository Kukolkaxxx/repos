using Microsoft.EntityFrameworkCore;
using Pogodnik.Data.Entities;

namespace Pogodnik.Data.Repositories
{
    public class SearchHistoryRepository : ISearchHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public SearchHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddSearchAsync(SearchHistory search)
        {
            await _context.SearchHistory.AddAsync(search);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<SearchHistory>> GetUserSearchHistoryAsync(string userId, int limit = 20)
        {
            return await _context.SearchHistory
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.SearchedAt)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<IEnumerable<SearchHistory>> GetRecentSearchesAsync(int limit = 10)
        {
            return await _context.SearchHistory
                .OrderByDescending(s => s.SearchedAt)
                .Take(limit)
                .ToListAsync();
        }

        public async Task ClearUserHistoryAsync(string userId)
        {
            var userHistory = await _context.SearchHistory
                .Where(s => s.UserId == userId)
                .ToListAsync();

            _context.SearchHistory.RemoveRange(userHistory);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetTotalSearchesCountAsync()
        {
            return await _context.SearchHistory.CountAsync();
        }

        public async Task<IEnumerable<SearchHistory>> GetPopularCitiesAsync(int limit = 10)
        {
            return await _context.SearchHistory
                .GroupBy(s => new { s.CityName, s.CountryCode })
                .Select(g => new SearchHistory
                {
                    CityName = g.Key.CityName,
                    CountryCode = g.Key.CountryCode,
                    SearchedAt = g.Max(s => s.SearchedAt)
                })
                .OrderByDescending(s => s.SearchedAt)
                .Take(limit)
                .ToListAsync();
        }
    }
}