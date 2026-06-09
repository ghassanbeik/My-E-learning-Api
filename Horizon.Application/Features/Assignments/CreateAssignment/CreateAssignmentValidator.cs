
using FluentValidation;

namespace Horizon.Application.Features.Assignments.CreateAssignment
{
    public class CreateAssignmentValidator : AbstractValidator<CreateAssignmentCommand>
    {
        public CreateAssignmentValidator()
        {
            RuleFor(x => x.Dto.LessonId).NotEmpty();
            RuleFor(x => x.Dto.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Dto.Description).NotEmpty().MaximumLength(5000);
            RuleFor(x => x.Dto.TotalPoints).GreaterThan(0).LessThanOrEqualTo(1000);
            RuleFor(x => x.Dto.LatePenaltyPercentage)
                .InclusiveBetween(0, 100)
                .WithMessage("Late penalty must be between 0 and 100.");
            RuleFor(x => x.Dto.DueDate)
                .GreaterThan(DateTime.UtcNow)
                .When(x => x.Dto.DueDate.HasValue)
                .WithMessage("Due date must be in the future.");
            RuleFor(x => x.Dto.TimeLimitHours)
                .GreaterThan(0)
                .When(x => x.Dto.TimeLimitHours.HasValue)
                .WithMessage("Time limit must be greater than 0.");
        }
    }
}
