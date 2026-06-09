
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Lessons.GetLessonNotes
{
    public class GetLessonNotesHandler : IRequestHandler<GetLessonNotesQuery, Result<List<LessonNoteDto>>>
    {
        private readonly IUnitOfWork _uow;
        public GetLessonNotesHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<List<LessonNoteDto>>> Handle(GetLessonNotesQuery request, CancellationToken ct)
        {
            var notes = await _uow.LessonNotes.GetByUserAndLessonAsync(request.UserId, request.LessonId, ct);
            return Result<List<LessonNoteDto>>.Success(notes.Select(n =>
                new LessonNoteDto(n.Id, n.LessonId, n.Content, n.VideoTimestampSeconds, n.CreatedAt)).ToList());
        }
    }
}
