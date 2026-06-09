

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Auth.UploadAvatar
{
    public record UploadAvatarCommand(Guid UserId, Stream FileStream, string FileName, string ContentType) : IRequest<Result<string>>;

}
