using Horizon.API.Common;
using Horizon.Application.DTOs;
using Horizon.Application.Features.Lessons.AddBookmark;
using Horizon.Application.Features.Lessons.AddLessonNote;
using Horizon.Application.Features.Lessons.DeleteLessonNote;
using Horizon.Application.Features.Lessons.GetBookmarks;
using Horizon.Application.Features.Lessons.GetLessonNotes;
using Horizon.Application.Features.Lessons.RemoveBookmark;
using Horizon.Infrastructure.Seeding;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizon.API.Controllers
{
    [Route("api/lessons")]
    public class LessonsController : BaseController
    {
        private readonly IMediator _mediator;
        public LessonsController(IMediator mediator) => _mediator = mediator;

        /// <summary>Get notes for a lesson</summary>
        [HttpGet("{lessonId:guid}/notes")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<List<LessonNoteDto>>), 200)]
        public async Task<IActionResult> GetNotes(Guid lessonId, CancellationToken ct)
            => FromResult(await _mediator.Send(new GetLessonNotesQuery(lessonId, UserId), ct));

        /// <summary>Add a note to a lesson</summary>
        [HttpPost("{lessonId:guid}/notes")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<LessonNoteDto>), 201)]
        public async Task<IActionResult> AddNote(Guid lessonId, [FromBody] CreateLessonNoteDto dto, CancellationToken ct)
            => FromResult(await _mediator.Send(new AddLessonNoteCommand(lessonId, UserId, dto), ct));

        /// <summary>Delete a note</summary>
        [HttpDelete("notes/{noteId:guid}")]
        [Authorize]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteNote(Guid noteId, CancellationToken ct)
            => FromResult(await _mediator.Send(new DeleteLessonNoteCommand(noteId, UserId), ct));

        /// <summary>Add a bookmark to a lesson</summary>
        [HttpPost("{lessonId:guid}/bookmarks")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<LessonBookmarkDto>), 201)]
        public async Task<IActionResult> AddBookmark(Guid lessonId, [FromBody] CreateLessonBookmarkDto dto, CancellationToken ct)
            => FromResult(await _mediator.Send(new AddBookmarkCommand(lessonId, UserId, dto), ct));

        /// <summary>Remove a bookmark</summary>
        [HttpDelete("bookmarks/{bookmarkId:guid}")]
        [Authorize]
        [ProducesResponseType(200)]
        public async Task<IActionResult> RemoveBookmark(Guid bookmarkId, CancellationToken ct)
            => FromResult(await _mediator.Send(new RemoveBookmarkCommand(bookmarkId, UserId), ct));

        /// <summary>Get all bookmarks for a course</summary>
        [HttpGet("bookmarks/course/{courseId:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<List<LessonBookmarkDto>>), 200)]
        public async Task<IActionResult> GetBookmarks(Guid courseId, CancellationToken ct)
            => FromResult(await _mediator.Send(new GetBookmarksQuery(courseId, UserId), ct));
    }

}
