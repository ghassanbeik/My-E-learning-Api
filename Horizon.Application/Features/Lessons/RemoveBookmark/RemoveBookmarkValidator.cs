

using FluentValidation;

namespace Horizon.Application.Features.Lessons.RemoveBookmark
{
    public class RemoveBookmarkValidator : AbstractValidator<RemoveBookmarkCommand>
    {
        public RemoveBookmarkValidator()
        {
            RuleFor(x => x.BookmarkId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
