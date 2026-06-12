
using FluentValidation;

namespace Horizon.Application.Features.Discussions.UpvoteReply
{
    public class UpvoteReplyValidator : AbstractValidator<UpvoteReplyCommand>
    {
        public UpvoteReplyValidator()
        {
            RuleFor(x => x.ReplyId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
