namespace Horizon.Domain.Interfaces.Services.StorageServices
{
    public class FileUploadResult
    {
        public bool Success { get; set; }
        public string? FileUrl { get; set; }
        public string? FilePath { get; set; }
        public string? FileName { get; set; }
        public long? SizeBytes { get; set; }
        public string? Error { get; set; }
    }
}
