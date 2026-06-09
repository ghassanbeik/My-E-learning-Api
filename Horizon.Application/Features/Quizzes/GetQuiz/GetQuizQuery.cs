

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Quizzes.GetQuiz
{
    public record GetQuizQuery(Guid QuizId, Guid UserId) : IRequest<Result<QuizDetailDto>>;
}
