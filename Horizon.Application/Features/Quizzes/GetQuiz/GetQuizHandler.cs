

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Quizzes.GetQuiz
{
    public class GetQuizHandler : IRequestHandler<GetQuizQuery, Result<QuizDetailDto>>
    {
        private readonly IUnitOfWork _uow;
        public GetQuizHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<QuizDetailDto>> Handle(GetQuizQuery request, CancellationToken ct)
        {
            var quiz = await _uow.Quizzes.GetWithQuestionsAsync(request.QuizId, ct);
            if (quiz == null) return Result<QuizDetailDto>.NotFound("Quiz not found.");

            var questions = quiz.Questions.OrderBy(q => q.DisplayOrder).Select(q =>
            {
                var bestAttempt = _uow.QuizAttempts
                    .GetBestAttemptAsync(request.UserId, quiz.Id, ct).Result;
                var showAnswers = quiz.ShowCorrectAnswers && bestAttempt != null;

                return new QuestionDto(
                    q.Id, q.Text, q.Explanation, q.Points, q.DisplayOrder,
                    q.AnswerOptions.OrderBy(a => a.DisplayOrder).Select(a =>
                        new AnswerOptionDto(a.Id, a.Text,
                            showAnswers ? a.IsCorrect : null,
                            a.DisplayOrder)).ToList());
            }).ToList();

            return Result<QuizDetailDto>.Success(new QuizDetailDto(
                quiz.Id, quiz.LessonId, quiz.Title, quiz.Instructions,
                quiz.TimeLimitMinutes, quiz.PassingScore, quiz.MaxAttempts,
                quiz.ShuffleQuestions, quiz.ShowCorrectAnswers, questions));
        }
    }
}
