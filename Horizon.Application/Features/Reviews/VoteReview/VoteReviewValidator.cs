
using FluentValidation;

namespace Horizon.Application.Features.Reviews.VoteReview
{
    public class VoteReviewValidator : AbstractValidator<VoteReviewCommand>
    {
        public VoteReviewValidator()
        {
            RuleFor(x => x.ReviewId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
