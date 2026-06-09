

using FluentValidation;

namespace Horizon.Application.Features.Categories.CreateCategory
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo(0);
        }
    }
}
