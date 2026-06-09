using Horizon.API.Common;
using Horizon.Application.DTOs;
using Horizon.Application.Features.Quizzes.CreateQuiz;
using Horizon.Application.Features.Quizzes.GetQuiz;
using Horizon.Application.Features.Quizzes.GetQuizAttempts;
using Horizon.Application.Features.Quizzes.SubmitQuiz;
using Horizon.Infrastructure.Seeding;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizon.API.Controllers
{
    [Route("api/quizzes")]
    public class QuizzesController : BaseController
    {
        private readonly IMediator _mediator;
        public QuizzesController(IMediator mediator) => _mediator = mediator;

        /// <summary>Get quiz with questions</summary>
        [HttpGet("{id:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<QuizDetailDto>), 200)]
        public async Task<IActionResult> GetQuiz(Guid id, CancellationToken ct)
            => FromResult(await _mediator.Send(new GetQuizQuery(id, UserId), ct));

        /// <summary>Create a quiz (Instructor)</summary>
        [HttpPost]
        [Authorize(Policy = "Instructor")]
        [ProducesResponseType(typeof(ApiResponse<QuizDto>), 201)]
        public async Task<IActionResult> Create([FromBody] CreateQuizDto dto, CancellationToken ct)
            => FromResult(await _mediator.Send(new CreateQuizCommand(UserId, dto), ct));

        /// <summary>Submit quiz answers</summary>
        [HttpPost("{id:guid}/submit")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<QuizAttemptDto>), 200)]
        public async Task<IActionResult> Submit(Guid id, [FromBody] SubmitQuizDto dto, CancellationToken ct)
            => FromResult(await _mediator.Send(new SubmitQuizCommand(id, UserId, dto), ct));

        /// <summary>Get my quiz attempts</summary>
        [HttpGet("{id:guid}/attempts")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<List<QuizAttemptDto>>), 200)]
        public async Task<IActionResult> GetAttempts(Guid id, CancellationToken ct)
            => FromResult(await _mediator.Send(new GetQuizAttemptsQuery(id, UserId), ct));
    }
}
