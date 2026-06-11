

using FluentValidation;

namespace Horizon.Application.Features.Lessons.AddBookmark
{
    public class AddBookmarkValidator : AbstractValidator<AddBookmarkCommand>
    {
        public AddBookmarkValidator()
        {
            RuleFor(x => x.Dto.Note).MaximumLength(1000).When(x => x.Dto.Note != null);
            RuleFor(x => x.Dto.VideoTimestampSeconds).GreaterThanOrEqualTo(0)
                .When(x => x.Dto.VideoTimestampSeconds.HasValue);
        }
    }
}
