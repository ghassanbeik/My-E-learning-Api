

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Discussions.GetCourseDiscussions
{
    public record GetCourseDiscussionsQuery(Guid CourseId, string? Type, int Page = 1, int PageSize = 20) : IRequest<Result<PagedResponse<DiscussionDto>>>;

}
