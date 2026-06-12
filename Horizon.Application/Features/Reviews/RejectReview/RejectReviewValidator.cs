

using FluentValidation;

namespace Horizon.Application.Features.Reviews.RejectReview
{
    public class RejectReviewValidator : AbstractValidator<RejectReviewCommand>
    {
        public RejectReviewValidator()
        {
            RuleFor(x => x.ReviewId).NotEmpty();
        }
    }
}
