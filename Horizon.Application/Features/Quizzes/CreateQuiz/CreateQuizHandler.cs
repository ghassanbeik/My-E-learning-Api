

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Entities;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Quizzes.CreateQuiz
{
    public class CreateQuizHandler : IRequestHandler<CreateQuizCommand, Result<QuizDto>>
    {
        private readonly IUnitOfWork _uow;
        public CreateQuizHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<QuizDto>> Handle(CreateQuizCommand request, CancellationToken ct)
        {
            var lesson = await _uow.Lessons.GetByIdAsync(request.Dto.LessonId, ct);
            if (lesson == null) return Result<QuizDto>.NotFound("Lesson not found.");

            var section = await _uow.Sections.GetByIdAsync(lesson.SectionId, ct);
            var course = section != null ? await _uow.Courses.GetByIdAsync(section.CourseId, ct) : null;
            if (course?.InstructorId != request.InstructorId) return Result<QuizDto>.Forbidden();

            var quiz = new Quiz
            {
                LessonId = request.Dto.LessonId,
                Title = request.Dto.Title,
                Instructions = request.Dto.Instructions,
                TimeLimitMinutes = request.Dto.TimeLimitMinutes,
                PassingScore = request.Dto.PassingScore,
                MaxAttempts = request.Dto.MaxAttempts,
                ShuffleQuestions = request.Dto.ShuffleQuestions,
                ShowCorrectAnswers = request.Dto.ShowCorrectAnswers,
            };

            await _uow.Quizzes.AddAsync(quiz, ct);
            await _uow.SaveChangesAsync(ct);

            foreach (var qDto in request.Dto.Questions)
            {
                var question = new Question
                {
                    QuizId = quiz.Id,
                    Text = qDto.Text,
                    Explanation = qDto.Explanation,
                    Points = qDto.Points,
                    DisplayOrder = qDto.DisplayOrder,
                };
                await _uow.Questions.AddAsync(question, ct);
                await _uow.SaveChangesAsync(ct);

                foreach (var aDto in qDto.AnswerOptions)
                    await _uow.AnswerOptions.AddAsync(new AnswerOption
                    {
                        QuestionId = question.Id,
                        Text = aDto.Text,
                        IsCorrect = aDto.IsCorrect,
                        DisplayOrder = aDto.DisplayOrder,
                    }, ct);
            }

            await _uow.SaveChangesAsync(ct);

            return Result<QuizDto>.Success(new QuizDto(
                quiz.Id, quiz.LessonId, quiz.Title, quiz.Instructions,
                quiz.TimeLimitMinutes, quiz.PassingScore, quiz.MaxAttempts,
                quiz.ShuffleQuestions, quiz.ShowCorrectAnswers,
                request.Dto.Questions.Count), 201);
        }
    }
}
