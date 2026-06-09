
using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Search.GetSearchSuggestions
{
    public record GetSearchSuggestionsQuery(string Query) : IRequest<Result<List<string>>>;

}
