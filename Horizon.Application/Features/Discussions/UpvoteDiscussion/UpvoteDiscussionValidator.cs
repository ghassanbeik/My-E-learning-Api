
using FluentValidation;

namespace Horizon.Application.Features.Discussions.UpvoteDiscussion
{
    public class UpvoteDiscussionValidator : AbstractValidator<UpvoteDiscussionCommand>
    {
        public UpvoteDiscussionValidator()
        {
            RuleFor(x => x.DiscussionId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
