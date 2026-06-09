

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Analytics.GetCourseAnalytics
{
    public record GetCourseAnalyticsQuery(Guid CourseId, Guid InstructorId, DateTime From, DateTime To) : IRequest<Result<CourseAnalyticsDto>>;

}
