

using FluentValidation;

namespace Horizon.Application.Features.WishlistCart.ClearCart
{
    public class ClearCartValidator : AbstractValidator<ClearCartCommand>
    {
        public ClearCartValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
