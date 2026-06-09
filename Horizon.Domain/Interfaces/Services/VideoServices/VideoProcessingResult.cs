namespace Horizon.Domain.Interfaces.Services.VideoServices
{
    public class VideoProcessingResult
    {
        public bool Success { get; set; }
        public string? ProcessedUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public int DurationSeconds { get; set; }
        public string? Error { get; set; }
    }
}
