namespace Horizon.Domain.Interfaces.Services.StorageServices
{
    public class FileUploadRequest
    {
        public Stream Content { get; set; } = Stream.Null;
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public string Folder { get; set; } = "uploads";
        public bool IsPublic { get; set; } = true;
        public long? MaxSizeBytes { get; set; }
    }
}
