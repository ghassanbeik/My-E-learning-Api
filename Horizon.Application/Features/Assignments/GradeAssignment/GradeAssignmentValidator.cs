

using FluentValidation;

namespace Horizon.Application.Features.Assignments.GradeAssignment
{
    public class GradeAssignmentValidator : AbstractValidator<GradeAssignmentCommand>
    {
        public GradeAssignmentValidator()
        {
            RuleFor(x => x.SubmissionId).NotEmpty();
            RuleFor(x => x.InstructorId).NotEmpty();
            RuleFor(x => x.Dto.Score).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Dto.Feedback)
                .MaximumLength(5000)
                .When(x => x.Dto.Feedback != null);
        }
    }
}
