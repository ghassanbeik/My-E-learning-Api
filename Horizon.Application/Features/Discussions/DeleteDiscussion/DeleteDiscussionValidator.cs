

using FluentValidation;

namespace Horizon.Application.Features.Discussions.DeleteDiscussion
{
    public class DeleteDiscussionValidator : AbstractValidator<DeleteDiscussionCommand>
    {
        public DeleteDiscussionValidator()
        {
            RuleFor(x => x.DiscussionId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
