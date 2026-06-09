

using FluentValidation;

namespace Horizon.Application.Features.Courses.UpdateCourse
{
    public class UpdateCourseValidator : AbstractValidator<UpdateCourseCommand>
    {
        public UpdateCourseValidator()
        {
            RuleFor(x => x.Dto.Title).MinimumLength(10).MaximumLength(200).When(x => x.Dto.Title != null);
            RuleFor(x => x.Dto.Description).MinimumLength(50).MaximumLength(5000).When(x => x.Dto.Description != null);
            RuleFor(x => x.Dto.Price).GreaterThanOrEqualTo(0).When(x => x.Dto.Price.HasValue);
            RuleFor(x => x.Dto.DiscountPrice)
                .LessThan(x => x.Dto.Price!.Value)
                .When(x => x.Dto.DiscountPrice.HasValue && x.Dto.Price.HasValue)
                .WithMessage("Discount price must be less than original price.");
        }
    }
}
