

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Quizzes.GetQuizAttempts
{
    public record GetQuizAttemptsQuery(Guid QuizId, Guid StudentId) : IRequest<Result<List<QuizAttemptDto>>>;

}
