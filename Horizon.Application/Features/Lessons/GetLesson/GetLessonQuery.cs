

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Lessons.GetLesson
{
    public record GetLessonQuery(Guid LessonId, Guid UserId) : IRequest<Result<LessonDto>>;

}
