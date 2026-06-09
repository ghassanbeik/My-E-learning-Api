

using FluentValidation;

namespace Horizon.Application.Features.Enrollments.UpdateProgress
{
    public class UpdateProgressValidator : AbstractValidator<UpdateProgressCommand>
    {
        public UpdateProgressValidator()
        {
            RuleFor(x => x.Dto.VideoWatchedSeconds).GreaterThanOrEqualTo(0).When(x => x.Dto.VideoWatchedSeconds.HasValue);
            RuleFor(x => x.Dto.VideoTotalSeconds).GreaterThanOrEqualTo(0).When(x => x.Dto.VideoTotalSeconds.HasValue);
            RuleFor(x => x.Dto.TimeSpentMinutes).GreaterThanOrEqualTo(0).When(x => x.Dto.TimeSpentMinutes.HasValue);
        }
    }
}
