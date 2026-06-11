

using FluentValidation;

namespace Horizon.Application.Features.Discussions.UpdateDiscussion
{
    public class UpdateDiscussionValidator : AbstractValidator<UpdateDiscussionCommand>
    {
        public UpdateDiscussionValidator()
        {
            RuleFor(x => x.Dto.Title).MinimumLength(5).MaximumLength(200).When(x => x.Dto.Title != null);
            RuleFor(x => x.Dto.Content).MinimumLength(10).MaximumLength(5000).When(x => x.Dto.Content != null);
        }
    }
}
