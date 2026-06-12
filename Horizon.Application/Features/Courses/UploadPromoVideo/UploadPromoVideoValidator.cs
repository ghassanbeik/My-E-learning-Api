

using FluentValidation;

namespace Horizon.Application.Features.Courses.UploadPromoVideo
{
    public class UploadPromoVideoValidator : AbstractValidator<UploadPromoVideoCommand>
    {
        private static readonly string[] _allowedTypes =
            { "video/mp4", "video/webm", "video/quicktime" };

        public UploadPromoVideoValidator()
        {
            RuleFor(x => x.CourseId).NotEmpty();
            RuleFor(x => x.InstructorId).NotEmpty();
            RuleFor(x => x.FileName).NotEmpty().MaximumLength(255);
            RuleFor(x => x.ContentType)
                .NotEmpty()
                .Must(t => _allowedTypes.Contains(t))
                .WithMessage("Only MP4, WebM, and MOV video formats are allowed.");
            RuleFor(x => x.FileStream)
                .NotNull()
                .WithMessage("Video file content is required.");
        }
    }
}
