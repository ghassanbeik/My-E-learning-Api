

using FluentValidation;

namespace Horizon.Application.Features.Certificates.DownloadCertificate
{
    public class DownloadCertificateValidator : AbstractValidator<DownloadCertificateCommand>
    {
        public DownloadCertificateValidator()
        {
            RuleFor(x => x.CertificateId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
