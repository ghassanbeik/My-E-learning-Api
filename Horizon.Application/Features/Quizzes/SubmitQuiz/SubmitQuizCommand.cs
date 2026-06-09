
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Quizzes.SubmitQuiz
{
    public record SubmitQuizCommand(Guid QuizId, Guid StudentId, SubmitQuizDto Dto) : IRequest<Result<QuizAttemptDto>>;

}
