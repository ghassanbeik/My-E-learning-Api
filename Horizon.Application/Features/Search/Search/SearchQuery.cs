

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Search.Search
{
    public record SearchQuery(string Query, string Category = "All", int Page = 1, int PageSize = 20, Guid? UserId = null) : IRequest<Result<SearchResponseDto>>;
}
