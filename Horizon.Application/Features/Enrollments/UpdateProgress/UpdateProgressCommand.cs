

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Enrollments.UpdateProgress
{
    public record UpdateProgressCommand(Guid StudentId, Guid CourseId, Guid LessonId, UpdateProgressDto Dto) : IRequest<Result<ProgressDto>>;

}
