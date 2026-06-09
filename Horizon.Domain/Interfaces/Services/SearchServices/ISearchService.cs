namespace Horizon.Domain.Interfaces.Services.SearchServices
{
    public interface ISearchService
    {
        Task<SearchResult> SearchAsync(SearchRequest request, CancellationToken ct = default);
        Task<IEnumerable<string>> GetSuggestionsAsync(string query, CancellationToken ct = default);
        Task LogSearchAsync(string query, Guid? userId, int resultsCount, CancellationToken ct = default);
    }
}
