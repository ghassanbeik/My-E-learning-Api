

using FluentValidation;

namespace Horizon.Application.Features.Reviews.CreateReviewResponse
{
    public class CreateReviewResponseValidator : AbstractValidator<CreateReviewResponseCommand>
    {
        public CreateReviewResponseValidator()
        {
            RuleFor(x => x.Dto.Response).NotEmpty().MaximumLength(2000);
        }
    }
}
