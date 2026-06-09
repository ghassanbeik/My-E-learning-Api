

using Horizon.Application.Common;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.StorageServices;
using MediatR;

namespace Horizon.Application.Features.Auth.UploadAvatar
{
    public class UploadAvatarHandler : IRequestHandler<UploadAvatarCommand, Result<string>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IFileStorageService _storage;

        public UploadAvatarHandler(IUnitOfWork uow, IFileStorageService storage) { _uow = uow; _storage = storage; }

        public async Task<Result<string>> Handle(UploadAvatarCommand request, CancellationToken ct)
        {
            var user = await _uow.Users.GetByIdAsync(request.UserId, ct);
            if (user == null) return Result<string>.NotFound("User not found.");

            var result = await _storage.UploadAsync(new FileUploadRequest
            {
                Content = request.FileStream,
                FileName = request.FileName,
                ContentType = request.ContentType,
                Folder = "avatars",
            }, ct);

            if (!result.Success) return Result<string>.Failure(result.Error ?? "Upload failed.");

            if (user.AvatarUrl != null)
                await _storage.DeleteAsync(user.AvatarUrl, ct);

            user.AvatarUrl = result.FileUrl;
            await _uow.Users.UpdateAsync(user);
            await _uow.SaveChangesAsync(ct);

            return Result<string>.Success(result.FileUrl!);
        }
    }

}
