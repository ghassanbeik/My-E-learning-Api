
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Lessons.AddBookmark
{
    public record AddBookmarkCommand(Guid LessonId, Guid UserId, CreateLessonBookmarkDto Dto) : IRequest<Result<LessonBookmarkDto>>;

}
