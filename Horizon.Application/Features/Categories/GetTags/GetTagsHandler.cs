

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Categories.GetTags
{
    public class GetTagsHandler : IRequestHandler<GetTagsQuery, Result<List<TagDto>>>
    {
        private readonly IUnitOfWork _uow;
        public GetTagsHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<List<TagDto>>> Handle(GetTagsQuery request, CancellationToken ct)
        {
            var tags = string.IsNullOrEmpty(request.Query)
                ? await _uow.Tags.GetPopularTagsAsync(request.Count, ct)
                : await _uow.Tags.SearchTagsAsync(request.Query, ct);

            return Result<List<TagDto>>.Success(tags.Select(t =>
                new TagDto(t.Id, t.Name, t.Description, t.UsageCount)).ToList());
        }
    }
}
