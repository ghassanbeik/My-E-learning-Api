
using FluentValidation;

namespace Horizon.Application.Features.WishlistCart.MoveToCart
{
    public class MoveToCartValidator : AbstractValidator<MoveToCartCommand>
    {
        public MoveToCartValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.CourseId).NotEmpty();
        }
    }
}
