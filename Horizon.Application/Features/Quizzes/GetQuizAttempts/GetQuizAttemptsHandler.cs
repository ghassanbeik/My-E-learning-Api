

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Quizzes.GetQuizAttempts
{
    public class GetQuizAttemptsHandler
       : IRequestHandler<GetQuizAttemptsQuery, Result<List<QuizAttemptDto>>>
    {
        private readonly IUnitOfWork _uow;
        public GetQuizAttemptsHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<List<QuizAttemptDto>>> Handle(
            GetQuizAttemptsQuery request, CancellationToken ct)
        {
            var attempts = await _uow.QuizAttempts
                .GetByStudentAndQuizAsync(request.StudentId, request.QuizId, ct);

            return Result<List<QuizAttemptDto>>.Success(attempts.Select(a =>
                new QuizAttemptDto(a.Id, a.QuizId, a.Score, a.MaxScore,
                    a.IsPassed, a.AttemptNumber, a.StartedAt, a.CompletedAt,
                    new())).ToList());
        }
    }
}
