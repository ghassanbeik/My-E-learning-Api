

using FluentValidation;

namespace Horizon.Application.Features.Quizzes.CreateQuiz
{
    public class CreateQuizValidator : AbstractValidator<CreateQuizCommand>
    {
        public CreateQuizValidator()
        {
            RuleFor(x => x.Dto.LessonId).NotEmpty();
            RuleFor(x => x.Dto.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Dto.PassingScore).InclusiveBetween(0, 100);
            RuleFor(x => x.Dto.MaxAttempts).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Dto.TimeLimitMinutes).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Dto.Questions).NotEmpty()
                .WithMessage("Quiz must have at least one question.");
            RuleForEach(x => x.Dto.Questions).ChildRules(q =>
            {
                q.RuleFor(x => x.Text).NotEmpty().MaximumLength(1000);
                q.RuleFor(x => x.Points).GreaterThan(0);
                q.RuleFor(x => x.AnswerOptions).NotEmpty()
                    .WithMessage("Each question must have at least two answer options.")
                    .Must(opts => opts.Count >= 2)
                    .WithMessage("Each question must have at least two answer options.")
                    .Must(opts => opts.Any(o => o.IsCorrect))
                    .WithMessage("Each question must have at least one correct answer.");
            });
        }
    }
    }
