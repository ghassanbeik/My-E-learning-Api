
using FluentValidation;

namespace Horizon.Application.Features.Categories.DeleteCategory
{
    public class DeleteCategoryValidator : AbstractValidator<DeleteCategoryCommand>
    {
        public DeleteCategoryValidator()
        {
            RuleFor(x => x.CategoryId).NotEmpty();
        }
    }
}
