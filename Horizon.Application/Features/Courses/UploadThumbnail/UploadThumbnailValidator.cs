

using FluentValidation;

namespace Horizon.Application.Features.Courses.UploadThumbnail
{
    public class UploadThumbnailValidator : AbstractValidator<UploadThumbnailCommand>
    {
        private static readonly string[] AllowedTypes = { "image/jpeg", "image/png", "image/webp" };

        public UploadThumbnailValidator()
        {
            RuleFor(x => x.FileName).NotEmpty();
            RuleFor(x => x.ContentType)
                .Must(t => AllowedTypes.Contains(t))
                .WithMessage("Only JPEG, PNG, and WebP images are allowed.");
        }
    }
}
