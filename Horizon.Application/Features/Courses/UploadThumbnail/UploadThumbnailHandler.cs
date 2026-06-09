
using Horizon.Application.Common;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.StorageServices;
using MediatR;

namespace Horizon.Application.Features.Courses.UploadThumbnail
{
    public class UploadThumbnailHandler : IRequestHandler<UploadThumbnailCommand, Result<string>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IFileStorageService _storage;

        public UploadThumbnailHandler(IUnitOfWork uow, IFileStorageService storage) { _uow = uow; _storage = storage; }

        public async Task<Result<string>> Handle(UploadThumbnailCommand request, CancellationToken ct)
        {
            var course = await _uow.Courses.GetByIdAsync(request.CourseId, ct);
            if (course == null) return Result<string>.NotFound("Course not found.");
            if (course.InstructorId != request.InstructorId) return Result<string>.Forbidden();

            if (!string.IsNullOrEmpty(course.ThumbnailUrl))
                await _storage.DeleteAsync(course.ThumbnailUrl, ct);

            var result = await _storage.UploadAsync(new FileUploadRequest
            {
                Content = request.FileStream,
                FileName = request.FileName,
                ContentType = request.ContentType,
                Folder = "thumbnails",
            }, ct);

            if (!result.Success) return Result<string>.Failure(result.Error ?? "Upload failed.");

            course.ThumbnailUrl = result.FileUrl;
           await _uow.Courses.UpdateAsync(course);
            await _uow.SaveChangesAsync(ct);
            return Result<string>.Success(result.FileUrl!);
        }
    }
}
