
using FluentValidation;
using Horizon.Domain.Enums;

namespace Horizon.Application.Features.Discussions.CreateDiscussion
{
    public class CreateDiscussionValidator : AbstractValidator<CreateDiscussionCommand>
    {
        public CreateDiscussionValidator()
        {
            RuleFor(x => x.Dto.CourseId).NotEmpty();
            RuleFor(x => x.Dto.Title).NotEmpty().MinimumLength(5).MaximumLength(200);
            RuleFor(x => x.Dto.Content).NotEmpty().MinimumLength(10).MaximumLength(5000);
            RuleFor(x => x.Dto.Type).NotEmpty()
                .Must(t => Enum.TryParse<DiscussionType>(t, out _))
                .WithMessage("Invalid discussion type.");
        }
    }
}
