
using FluentValidation;
using Horizon.Domain.Enums;

namespace Horizon.Application.Features.Payments.CreateCoupon
{
    public class CreateCouponValidator : AbstractValidator<CreateCouponCommand>
    {
        public CreateCouponValidator()
        {
            RuleFor(x => x.Dto.Code).NotEmpty().MaximumLength(50)
                .Matches("^[A-Z0-9]+$").WithMessage("Coupon code must be uppercase letters and numbers only.");
            RuleFor(x => x.Dto.Type).Must(t => Enum.TryParse<CouponType>(t, out _))
                .WithMessage("Invalid coupon type.");
            RuleFor(x => x.Dto.Value).GreaterThan(0);
            RuleFor(x => x.Dto.Value).LessThanOrEqualTo(100)
                .When(x => x.Dto.Type == "Percentage")
                .WithMessage("Percentage discount cannot exceed 100%.");
            RuleFor(x => x.Dto.ExpiryDate).GreaterThan(DateTime.UtcNow)
                .When(x => x.Dto.ExpiryDate.HasValue)
                .WithMessage("Expiry date must be in the future.");
        }
    }
}
