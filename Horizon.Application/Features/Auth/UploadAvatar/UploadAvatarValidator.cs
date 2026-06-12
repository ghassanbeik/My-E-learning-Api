
using FluentValidation;

namespace Horizon.Application.Features.Auth.UploadAvatar
{
    public class UploadAvatarValidator : AbstractValidator<UploadAvatarCommand>
    {
        private static readonly string[] _allowedTypes =
            { "image/jpeg", "image/png", "image/webp" };

        public UploadAvatarValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.FileName).NotEmpty().MaximumLength(255);
            RuleFor(x => x.ContentType)
                .NotEmpty()
                .Must(t => _allowedTypes.Contains(t))
                .WithMessage("Only JPEG, PNG, and WebP images are allowed.");
            RuleFor(x => x.FileStream)
                .NotNull()
                .WithMessage("File content is required.");
        }
    }
}
