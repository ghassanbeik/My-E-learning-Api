
using FluentValidation;

namespace Horizon.Application.Features.Reviews.CreateReview
{
    public class CreateReviewValidator : AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewValidator()
        {
            RuleFor(x => x.Dto.CourseId).NotEmpty();
            RuleFor(x => x.Dto.Rating).InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");
            RuleFor(x => x.Dto.Comment).MaximumLength(2000).When(x => x.Dto.Comment != null);
        }
    }

}
