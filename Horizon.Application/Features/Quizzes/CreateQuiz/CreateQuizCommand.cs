

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Quizzes.CreateQuiz
{
    public record CreateQuizCommand(Guid InstructorId, CreateQuizDto Dto) : IRequest<Result<QuizDto>>;

}
