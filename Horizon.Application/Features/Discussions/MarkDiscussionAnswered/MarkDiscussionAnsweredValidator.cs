

using FluentValidation;

namespace Horizon.Application.Features.Discussions.MarkDiscussionAnswered
{
    public class MarkDiscussionAnsweredValidator : AbstractValidator<MarkDiscussionAnsweredCommand>
    {
        public MarkDiscussionAnsweredValidator()
        {
            RuleFor(x => x.DiscussionId).NotEmpty();
            RuleFor(x => x.ReplyId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
