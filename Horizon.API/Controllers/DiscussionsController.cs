using Horizon.API.Common;
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Application.Features.Discussions.CreateDiscussion;
using Horizon.Application.Features.Discussions.CreateReply;
using Horizon.Application.Features.Discussions.DeleteDiscussion;
using Horizon.Application.Features.Discussions.DeleteReply;
using Horizon.Application.Features.Discussions.GetCourseDiscussions;
using Horizon.Application.Features.Discussions.GetDiscussionDetail;
using Horizon.Application.Features.Discussions.GetLessonDiscussions;
using Horizon.Application.Features.Discussions.MarkDiscussionAnswered;
using Horizon.Application.Features.Discussions.PinDiscussion;
using Horizon.Application.Features.Discussions.UpdateDiscussion;
using Horizon.Application.Features.Discussions.UpvoteDiscussion;
using Horizon.Application.Features.Discussions.UpvoteReply;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizon.API.Controllers
{
    [Route("api/discussions")]
    public class DiscussionsController : BaseController
    {
        private readonly IMediator _mediator;
        public DiscussionsController(IMediator mediator) => _mediator = mediator;

        /// <summary>Get discussions for a course</summary>
        [HttpGet("course/{courseId:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<PagedResponse<DiscussionDto>>), 200)]
        public async Task<IActionResult> GetCourseDiscussions(Guid courseId, [FromQuery] string? type, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
            => FromResult(await _mediator.Send(new GetCourseDiscussionsQuery(courseId, type, page, pageSize), ct));

        /// <summary>Get discussions for a lesson</summary>
        [HttpGet("lesson/{lessonId:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<PagedResponse<DiscussionDto>>), 200)]
        public async Task<IActionResult> GetLessonDiscussions(Guid lessonId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
            => FromResult(await _mediator.Send(new GetLessonDiscussionsQuery(lessonId, page, pageSize), ct));

        /// <summary>Get discussion detail with replies</summary>
        [HttpGet("{id:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<DiscussionDto>), 200)]
        public async Task<IActionResult> GetDetail(Guid id, CancellationToken ct)
            => FromResult(await _mediator.Send(new GetDiscussionDetailQuery(id), ct));

        /// <summary>Create a discussion</summary>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<DiscussionDto>), 201)]
        public async Task<IActionResult> Create([FromBody] CreateDiscussionDto dto, CancellationToken ct)
            => FromResult(await _mediator.Send(new CreateDiscussionCommand(UserId, dto), ct));

        /// <summary>Update a discussion</summary>
        [HttpPut("{id:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<DiscussionDto>), 200)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDiscussionDto dto, CancellationToken ct)
            => FromResult(await _mediator.Send(new UpdateDiscussionCommand(id, UserId, dto), ct));

        /// <summary>Delete a discussion</summary>
        [HttpDelete("{id:guid}")]
        [Authorize]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
            => FromResult(await _mediator.Send(new DeleteDiscussionCommand(id, UserId), ct));

        /// <summary>Upvote a discussion</summary>
        [HttpPost("{id:guid}/upvote")]
        [Authorize]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Upvote(Guid id, CancellationToken ct)
            => FromResult(await _mediator.Send(new UpvoteDiscussionCommand(id, UserId), ct));

        /// <summary>Pin/unpin a discussion</summary>
        [HttpPost("{id:guid}/pin")]
        [Authorize(Policy = "Instructor")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Pin(Guid id, CancellationToken ct)
            => FromResult(await _mediator.Send(new PinDiscussionCommand(id, UserId), ct));

        /// <summary>Mark discussion as answered</summary>
        [HttpPost("{id:guid}/mark-answered/{replyId:guid}")]
        [Authorize]
        [ProducesResponseType(200)]
        public async Task<IActionResult> MarkAnswered(Guid id, Guid replyId, CancellationToken ct)
            => FromResult(await _mediator.Send(new MarkDiscussionAnsweredCommand(id, replyId, UserId), ct));

        /// <summary>Create a reply</summary>
        [HttpPost("{id:guid}/replies")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<DiscussionReplyDto>), 201)]
        public async Task<IActionResult> Reply(Guid id, [FromBody] CreateDiscussionReplyDto dto, CancellationToken ct)
            => FromResult(await _mediator.Send(new CreateReplyCommand(id, UserId, dto), ct));

        /// <summary>Delete a reply</summary>
        [HttpDelete("replies/{replyId:guid}")]
        [Authorize]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteReply(Guid replyId, CancellationToken ct)
            => FromResult(await _mediator.Send(new DeleteReplyCommand(replyId, UserId), ct));

        /// <summary>Upvote a reply</summary>
        [HttpPost("replies/{replyId:guid}/upvote")]
        [Authorize]
        [ProducesResponseType(200)]
        public async Task<IActionResult> UpvoteReply(Guid replyId, CancellationToken ct)
            => FromResult(await _mediator.Send(new UpvoteReplyCommand(replyId, UserId), ct));
    }

}
