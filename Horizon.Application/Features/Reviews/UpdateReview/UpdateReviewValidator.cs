

using FluentValidation;

namespace Horizon.Application.Features.Reviews.UpdateReview
{
    public class UpdateReviewValidator : AbstractValidator<UpdateReviewCommand>
    {
        public UpdateReviewValidator()
        {
            RuleFor(x => x.Dto.Rating).InclusiveBetween(1, 5).When(x => x.Dto.Rating.HasValue);
            RuleFor(x => x.Dto.Comment).MaximumLength(2000).When(x => x.Dto.Comment != null);
        }
    }
}
