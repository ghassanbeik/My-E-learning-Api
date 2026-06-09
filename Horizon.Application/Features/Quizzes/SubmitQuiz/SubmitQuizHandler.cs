

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Entities;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Quizzes.SubmitQuiz
{
    public class SubmitQuizHandler : IRequestHandler<SubmitQuizCommand, Result<QuizAttemptDto>>
    {
        private readonly IUnitOfWork _uow;
        public SubmitQuizHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<QuizAttemptDto>> Handle(SubmitQuizCommand request, CancellationToken ct)
        {
            var quiz = await _uow.Quizzes.GetWithQuestionsAsync(request.QuizId, ct);
            if (quiz == null) return Result<QuizAttemptDto>.NotFound("Quiz not found.");

            var attemptCount = await _uow.QuizAttempts.GetAttemptCountAsync(request.StudentId, request.QuizId, ct);
            if (quiz.MaxAttempts > 0 && attemptCount >= quiz.MaxAttempts)
                return Result<QuizAttemptDto>.Failure($"Maximum attempts ({quiz.MaxAttempts}) reached.");

            var answers = new List<QuizAnswer>();
            int totalScore = 0;
            int maxScore = quiz.Questions.Sum(q => q.Points);
            var answerResults = new List<QuizAnswerResultDto>();

            foreach (var submission in request.Dto.Answers)
            {
                var question = quiz.Questions.FirstOrDefault(q => q.Id == submission.QuestionId);
                if (question == null) continue;

                var selected = question.AnswerOptions.FirstOrDefault(a => a.Id == submission.SelectedAnswerId);
                var isCorrect = selected?.IsCorrect ?? false;
                var points = isCorrect ? question.Points : 0;
                totalScore += points;

                answers.Add(new QuizAnswer
                {
                    QuestionId = submission.QuestionId,
                    SelectedAnswerId = submission.SelectedAnswerId,
                    IsCorrect = isCorrect,
                    PointsEarned = points,
                });

                var correctAnswer = question.AnswerOptions.FirstOrDefault(a => a.IsCorrect);
                answerResults.Add(new QuizAnswerResultDto(
                    question.Id, question.Text, submission.SelectedAnswerId,
                    selected?.Text ?? string.Empty, isCorrect, points,
                    question.Explanation, correctAnswer?.Text));
            }

            var passed = maxScore > 0 && (double)totalScore / maxScore * 100 >= quiz.PassingScore;
            var attempt = new QuizAttempt
            {
                QuizId = request.QuizId,
                StudentId = request.StudentId,
                Score = totalScore,
                MaxScore = maxScore,
                IsPassed = passed,
                AttemptNumber = attemptCount + 1,
                StartedAt = DateTime.UtcNow,
                CompletedAt = DateTime.UtcNow,
                Answers = answers,
            };

            await _uow.QuizAttempts.AddAsync(attempt, ct);
            await _uow.SaveChangesAsync(ct);

            return Result<QuizAttemptDto>.Success(new QuizAttemptDto(
                attempt.Id, attempt.QuizId, attempt.Score, attempt.MaxScore,
                attempt.IsPassed, attempt.AttemptNumber, attempt.StartedAt,
                attempt.CompletedAt, answerResults));
        }
    }

}
