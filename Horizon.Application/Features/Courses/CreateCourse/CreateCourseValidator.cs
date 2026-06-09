

using FluentValidation;
using Horizon.Domain.Enums;

namespace Horizon.Application.Features.Courses.CreateCourse
{
    public class CreateCourseValidator : AbstractValidator<CreateCourseCommand>
    {
        public CreateCourseValidator()
        {
            RuleFor(x => x.Dto.Title).NotEmpty().MinimumLength(10).MaximumLength(200);
            RuleFor(x => x.Dto.Description).NotEmpty().MinimumLength(50).MaximumLength(5000);
            RuleFor(x => x.Dto.Level).NotEmpty()
                .Must(l => Enum.TryParse<CourseLevel>(l, out _))
                .WithMessage("Invalid course level.");
            RuleFor(x => x.Dto.Price).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Dto.DiscountPrice)
                .LessThan(x => x.Dto.Price)
                .When(x => x.Dto.DiscountPrice.HasValue)
                .WithMessage("Discount price must be less than original price.");
            RuleFor(x => x.Dto.CategoryIds).NotEmpty().WithMessage("At least one category is required.");
        }
    }
}
