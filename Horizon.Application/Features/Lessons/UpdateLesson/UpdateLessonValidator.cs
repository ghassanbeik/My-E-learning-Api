

using FluentValidation;

namespace Horizon.Application.Features.Lessons.UpdateLesson
{
    public class UpdateLessonValidator : AbstractValidator<UpdateLessonCommand>
    {
        public UpdateLessonValidator()
        {
            RuleFor(x => x.Dto.Title).MaximumLength(200).When(x => x.Dto.Title != null);
            RuleFor(x => x.Dto.DurationMinutes).GreaterThanOrEqualTo(0).When(x => x.Dto.DurationMinutes.HasValue);
            RuleFor(x => x.Dto.VideoUrl)
                .Must(u => u == null || Uri.TryCreate(u, UriKind.Absolute, out _))
                .When(x => x.Dto.VideoUrl != null)
                .WithMessage("Video URL must be a valid URL.");
        }
    }
}
