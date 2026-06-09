

using FluentValidation;

namespace Horizon.Application.Features.Lessons.AddLessonNote
{
    public class AddLessonNoteValidator : AbstractValidator<AddLessonNoteCommand>
    {
        public AddLessonNoteValidator()
        {
            RuleFor(x => x.Dto.Content).NotEmpty().MaximumLength(5000);
            RuleFor(x => x.Dto.VideoTimestampSeconds).GreaterThanOrEqualTo(0)
                .When(x => x.Dto.VideoTimestampSeconds.HasValue);
        }
    }
}
