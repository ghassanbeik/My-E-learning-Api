

using FluentValidation;

namespace Horizon.Application.Features.WishlistCart.RemoveFromWishlist
{
    public class RemoveFromWishlistValidator : AbstractValidator<RemoveFromWishlistCommand>
    {
        public RemoveFromWishlistValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.CourseId).NotEmpty();
        }
    }
}
