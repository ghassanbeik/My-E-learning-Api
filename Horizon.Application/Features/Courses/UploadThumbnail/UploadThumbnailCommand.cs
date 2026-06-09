

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Courses.UploadThumbnail
{
    public record UploadThumbnailCommand(Guid CourseId, Guid InstructorId, Stream FileStream, string FileName, string ContentType) : IRequest<Result<string>>;

}
