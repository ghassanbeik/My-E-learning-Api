

using FluentValidation;

namespace Horizon.Application.Features.WishlistCart.AddToCart
{
    public class AddToCartValidator : AbstractValidator<AddToCartCommand>
    {
        public AddToCartValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.CourseId).NotEmpty();
            RuleFor(x => x.CouponCode).MaximumLength(50)
                .When(x => x.CouponCode != null);
        }
    }
}
