
using FluentValidation;
using Horizon.Domain.Enums;

namespace Horizon.Application.Features.Lessons.CreateLesson
{
    public class CreateLessonValidator : AbstractValidator<CreateLessonCommand>
    {
        public CreateLessonValidator()
        {
            RuleFor(x => x.Dto.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Dto.ContentType).NotEmpty()
                .Must(t => Enum.TryParse<LessonContentType>(t, out _))
                .WithMessage("Invalid content type.");
            RuleFor(x => x.Dto.DurationMinutes).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Dto.DisplayOrder).GreaterThanOrEqualTo(0);
        }
    }
}
