
using FluentValidation;

namespace Horizon.Application.Features.Discussions.PinDiscussion
{
    public class PinDiscussionValidator : AbstractValidator<PinDiscussionCommand>
    {
        public PinDiscussionValidator()
        {
            RuleFor(x => x.DiscussionId).NotEmpty();
            RuleFor(x => x.InstructorId).NotEmpty();
        }
    }
}
