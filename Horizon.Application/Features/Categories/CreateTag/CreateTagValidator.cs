

using FluentValidation;

namespace Horizon.Application.Features.Categories.CreateTag
{
    public class CreateTagValidator : AbstractValidator<CreateTagCommand>
    {
        public CreateTagValidator()
        {
            RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(100);
        }
    }
}
