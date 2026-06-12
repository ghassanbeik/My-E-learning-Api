
using FluentValidation;

namespace Horizon.Application.Features.Discussions.DeleteReply
{
    public class DeleteReplyValidator : AbstractValidator<DeleteReplyCommand>
    {
        public DeleteReplyValidator()
        {
            RuleFor(x => x.ReplyId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
