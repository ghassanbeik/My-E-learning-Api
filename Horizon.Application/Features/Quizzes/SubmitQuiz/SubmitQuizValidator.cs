
using FluentValidation;

namespace Horizon.Application.Features.Quizzes.SubmitQuiz
{
    public class SubmitQuizValidator : AbstractValidator<SubmitQuizCommand>
    {
        public SubmitQuizValidator()
        {
            RuleFor(x => x.Dto.Answers).NotEmpty().WithMessage("Must provide at least one answer.");
            RuleForEach(x => x.Dto.Answers).ChildRules(a =>
            {
                a.RuleFor(x => x.QuestionId).NotEmpty();
                a.RuleFor(x => x.SelectedAnswerId).NotEmpty();
            });
        }
    }
}
