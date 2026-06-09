

using FluentValidation;

namespace Horizon.Application.Features.Assignments.SubmitAssignment
{
    public class SubmitAssignmentValidator : AbstractValidator<SubmitAssignmentCommand>
    {
        public SubmitAssignmentValidator()
        {
            RuleFor(x => x.Dto).Must(d => !string.IsNullOrEmpty(d.SubmissionText) || !string.IsNullOrEmpty(d.FileUrl))
                .WithMessage("Must provide either text or file submission.");
            RuleFor(x => x.Dto.SubmissionText).MaximumLength(10000).When(x => x.Dto.SubmissionText != null);
        }
    }
}
