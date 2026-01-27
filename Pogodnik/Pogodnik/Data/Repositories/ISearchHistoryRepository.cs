using Pogodnik.Data.Entities;

namespace Pogodnik.Data.Repositories
{
    public interface ISearchHistoryRepository
    {
        Task AddSearchAsync(SearchHistory search);
        Task<IEnumerable<SearchHistory>> GetUserSearchHistoryAsync(string userId, int limit = 20);
        Task<IEnumerable<SearchHistory>> GetRecentSearchesAsync(int limit = 10);
        Task ClearUserHistoryAsync(string userId);
        Task<int> GetTotalSearchesCountAsync();
        Task<IEnumerable<SearchHistory>> GetPopularCitiesAsync(int limit = 10);
    }
}