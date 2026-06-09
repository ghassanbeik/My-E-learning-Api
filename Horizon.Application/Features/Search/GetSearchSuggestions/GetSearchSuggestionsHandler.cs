
using Horizon.Application.Common;
using Horizon.Domain.Interfaces.Services.SearchServices;
using MediatR;

namespace Horizon.Application.Features.Search.GetSearchSuggestions
{
    public class GetSearchSuggestionsHandler : IRequestHandler<GetSearchSuggestionsQuery, Result<List<string>>>
    {
        private readonly ISearchService _search;
        public GetSearchSuggestionsHandler(ISearchService search) => _search = search;

        public async Task<Result<List<string>>> Handle(GetSearchSuggestionsQuery request, CancellationToken ct)
        {
            var suggestions = await _search.GetSuggestionsAsync(request.Query, ct);
            return Result<List<string>>.Success(suggestions.ToList());
        }
    }
}
