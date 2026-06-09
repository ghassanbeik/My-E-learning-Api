using Horizon.API.Common;
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Application.Features.Reviews.ApproveReview;
using Horizon.Application.Features.Reviews.CreateReview;
using Horizon.Application.Features.Reviews.CreateReviewResponse;
using Horizon.Application.Features.Reviews.DeleteReview;
using Horizon.Application.Features.Reviews.GetCourseReviews;
using Horizon.Application.Features.Reviews.GetPendingReviews;
using Horizon.Application.Features.Reviews.GetRatingDistribution;
using Horizon.Application.Features.Reviews.RejectReview;
using Horizon.Application.Features.Reviews.UpdateReview;
using Horizon.Application.Features.Reviews.VoteReview;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizon.API.Controllers
{

    [Route("api/reviews")]
    public class ReviewsController : BaseController
    {
        private readonly IMediator _mediator;
        public ReviewsController(IMediator mediator) => _mediator = mediator;

        /// <summary>Get reviews for a course</summary>
        [HttpGet("course/{courseId:guid}")]
        [ProducesResponseType(typeof(ApiResponse<PagedResponse<ReviewDto>>), 200)]
        public async Task<IActionResult> GetCourseReviews(Guid courseId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
            => FromResult(await _mediator.Send(new GetCourseReviewsQuery(courseId, page, pageSize), ct));

        /// <summary>Get rating distribution for a course</summary>
        [HttpGet("course/{courseId:guid}/distribution")]
        [ProducesResponseType(typeof(ApiResponse<Dictionary<int, int>>), 200)]
        public async Task<IActionResult> GetRatingDistribution(Guid courseId, CancellationToken ct)
            => FromResult(await _mediator.Send(new GetRatingDistributionQuery(courseId), ct));

        /// <summary>Submit a review</summary>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<ReviewDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse), 403)]
        [ProducesResponseType(typeof(ApiResponse), 409)]
        public async Task<IActionResult> Create([FromBody] CreateReviewDto dto, CancellationToken ct)
            => FromResult(await _mediator.Send(new CreateReviewCommand(UserId, dto), ct));

        /// <summary>Update a review</summary>
        [HttpPut("{id:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<ReviewDto>), 200)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateReviewDto dto, CancellationToken ct)
            => FromResult(await _mediator.Send(new UpdateReviewCommand(id, UserId, dto), ct));

        /// <summary>Delete a review</summary>
        [HttpDelete("{id:guid}")]
        [Authorize]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
            => FromResult(await _mediator.Send(new DeleteReviewCommand(id, UserId), ct));

        /// <summary>Vote a review as helpful</summary>
        [HttpPost("{id:guid}/vote")]
        [Authorize]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Vote(Guid id, [FromBody] bool isHelpful, CancellationToken ct)
            => FromResult(await _mediator.Send(new VoteReviewCommand(id, UserId, isHelpful), ct));

        /// <summary>Respond to a review (Instructor)</summary>
        [HttpPost("{id:guid}/response")]
        [Authorize(Policy = "Instructor")]
        [ProducesResponseType(typeof(ApiResponse<ReviewResponseDto>), 201)]
        public async Task<IActionResult> Respond(Guid id, [FromBody] CreateReviewResponseDto dto, CancellationToken ct)
            => FromResult(await _mediator.Send(new CreateReviewResponseCommand(id, UserId, dto), ct));

        /// <summary>Get pending reviews (Admin)</summary>
        [HttpGet("pending")]
        [Authorize(Policy = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<PagedResponse<ReviewDto>>), 200)]
        public async Task<IActionResult> GetPending([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
            => FromResult(await _mediator.Send(new GetPendingReviewsQuery(page, pageSize), ct));

        /// <summary>Approve a review (Admin)</summary>
        [HttpPost("{id:guid}/approve")]
        [Authorize(Policy = "Admin")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Approve(Guid id, CancellationToken ct)
            => FromResult(await _mediator.Send(new ApproveReviewCommand(id), ct));

        /// <summary>Reject a review (Admin)</summary>
        [HttpPost("{id:guid}/reject")]
        [Authorize(Policy = "Admin")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Reject(Guid id, CancellationToken ct)
            => FromResult(await _mediator.Send(new RejectReviewCommand(id), ct));
    }
}
