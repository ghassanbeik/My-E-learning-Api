
using FluentValidation;

namespace Horizon.Application.Features.Categories.UpdateCategory
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Dto.Name).MaximumLength(100).When(x => x.Dto.Name != null);
            RuleFor(x => x.Dto.Color).MaximumLength(50).When(x => x.Dto.Color != null);
        }
    }
}
