

using FluentValidation;

namespace Horizon.Application.Features.WishlistCart.RemoveFromCart
{
    public class RemoveFromCartValidator : AbstractValidator<RemoveFromCartCommand>
    {
        public RemoveFromCartValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.CourseId).NotEmpty();
        }
    }
}
