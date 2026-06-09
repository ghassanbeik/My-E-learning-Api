

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Courses.UploadPromoVideo
{
    public record UploadPromoVideoCommand(Guid CourseId, Guid InstructorId, Stream FileStream, string FileName, string ContentType) : IRequest<Result<string>>;

}
