

using FluentValidation;

namespace Horizon.Application.Features.WishlistCart.AddToWishlist
{
    public class AddToWishlistValidator : AbstractValidator<AddToWishlistCommand>
    {
        public AddToWishlistValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.CourseId).NotEmpty();
        }
    }
}
