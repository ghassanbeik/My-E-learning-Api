

using FluentValidation;

namespace Horizon.Application.Features.Reviews.ApproveReview
{
    public class ApproveReviewValidator : AbstractValidator<ApproveReviewCommand>
    {
        public ApproveReviewValidator()
        {
            RuleFor(x => x.ReviewId).NotEmpty();
        }
    }
}
