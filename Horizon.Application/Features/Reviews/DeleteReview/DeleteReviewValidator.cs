

using FluentValidation;

namespace Horizon.Application.Features.Reviews.DeleteReview
{
    public class DeleteReviewValidator : AbstractValidator<DeleteReviewCommand>
    {
        public DeleteReviewValidator()
        {
            RuleFor(x => x.ReviewId).NotEmpty();
            RuleFor(x => x.StudentId).NotEmpty();
        }
    }
}
