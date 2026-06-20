using Horizon.API.Common;
using Horizon.Application.DTOs;
using Horizon.Application.Features.Assignments.CreateAssignment;
using Horizon.Application.Features.Assignments.GetAssignmentSubmissions;
using Horizon.Application.Features.Assignments.GetMySubmission;
using Horizon.Application.Features.Assignments.GradeAssignment;
using Horizon.Application.Features.Assignments.SubmitAssignment;
using Horizon.Domain.Interfaces.Services.StorageServices;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Horizon.API.Controllers
{
    [Route("api/assignments")]
    public class AssignmentsController : BaseController
    {
        private readonly IMediator _mediator;
        public AssignmentsController(IMediator mediator) => _mediator = mediator;

        /// <summary>Create an assignment (Instructor)</summary>
        [HttpPost]
        [Authorize(Policy = "Instructor")]
        [ProducesResponseType(typeof(ApiResponse<AssignmentDto>), 201)]
        public async Task<IActionResult> Create([FromBody] CreateAssignmentDto dto, CancellationToken ct)
            => FromResult(await _mediator.Send(new CreateAssignmentCommand(UserId, dto), ct));

        /// <summary>Submit assignment</summary>
        [HttpPost("{id:guid}/submit")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<AssignmentSubmissionDto>), 200)]
        public async Task<IActionResult> Submit(Guid id, [FromBody] SubmitAssignmentDto dto, CancellationToken ct)
            => FromResult(await _mediator.Send(new SubmitAssignmentCommand(id, UserId, dto), ct));

        /// <summary>Grade a submission (Instructor)</summary>
        [HttpPost("submissions/{submissionId:guid}/grade")]
        [Authorize(Policy = "Instructor")]
        [ProducesResponseType(typeof(ApiResponse<AssignmentSubmissionDto>), 200)]
        public async Task<IActionResult> Grade(Guid submissionId, [FromBody] GradeAssignmentDto dto, CancellationToken ct)
            => FromResult(await _mediator.Send(new GradeAssignmentCommand(submissionId, UserId, dto), ct));

        /// <summary>Get submissions for an assignment (Instructor)</summary>
        [HttpGet("{id:guid}/submissions")]
        [Authorize(Policy = "Instructor")]
        [ProducesResponseType(typeof(ApiResponse<List<AssignmentSubmissionDto>>), 200)]
        public async Task<IActionResult> GetSubmissions(Guid id, CancellationToken ct)
            => FromResult(await _mediator.Send(new GetAssignmentSubmissionsQuery(id, UserId), ct));

        /// <summary>Get my submission for an assignment</summary>
        [HttpGet("{id:guid}/my-submission")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<AssignmentSubmissionDto>), 200)]
        public async Task<IActionResult> GetMySubmission(Guid id, CancellationToken ct)
            => FromResult(await _mediator.Send(new GetMySubmissionQuery(id, UserId), ct));

        /// <summary>Upload assignment file</summary>
            [EnableRateLimiting("upload")]
    [HttpPost("{id:guid}/upload")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> UploadFile(Guid id, IFormFile file, CancellationToken ct)
        {
            if (file == null || file.Length == 0)
                return BadRequest(ApiResponse.Failure("No file uploaded."));

            if (file.Length > 50 * 1024 * 1024)
                return BadRequest(ApiResponse.Failure("File size must not exceed 50MB."));

            var storage = HttpContext.RequestServices
                .GetRequiredService<IFileStorageService>();

            var result = await storage.UploadAsync(new FileUploadRequest
            {
                Content = file.OpenReadStream(),
                FileName = file.FileName,
                ContentType = file.ContentType,
                Folder = $"assignments/{id}",
            }, ct);

            if (!result.Success)
                return BadRequest(ApiResponse.Failure(result.Error ?? "Upload failed."));

            return Ok(ApiResponse<string>.Success(result.FileUrl!));
        }
    }
}
