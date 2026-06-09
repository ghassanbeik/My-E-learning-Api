

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Discussions.GetLessonDiscussions
{
    public record GetLessonDiscussionsQuery(Guid LessonId, int Page = 1, int PageSize = 20) : IRequest<Result<PagedResponse<DiscussionDto>>>;

}
