

using FluentValidation;

namespace Horizon.Application.Features.Discussions.CreateReply
{
    public class CreateReplyValidator : AbstractValidator<CreateReplyCommand>
    {
        public CreateReplyValidator()
        {
            RuleFor(x => x.Dto.Content).NotEmpty().MinimumLength(2).MaximumLength(5000);
        }
    }
}
