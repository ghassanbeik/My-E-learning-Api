namespace Horizon.Domain.Interfaces.Services.VideoServices
{
    public interface IVideoProcessingService
    {
        Task<VideoProcessingResult> ProcessAsync(string videoUrl, CancellationToken ct = default);
        Task<string> GenerateThumbnailAsync(string videoUrl, int timestampSeconds = 5, CancellationToken ct = default);
        Task<int> GetDurationAsync(string videoUrl, CancellationToken ct = default);
        Task<bool> IsValidVideoAsync(string videoUrl, CancellationToken ct = default);
    }
}
