
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Categories.GetTags
{
    public record GetTagsQuery(string? Query, int Count = 20) : IRequest<Result<List<TagDto>>>;

}
