namespace Horizon.Domain.Interfaces.Services.StorageServices
{
    public interface IFileStorageService
    {
        Task<FileUploadResult> UploadAsync(FileUploadRequest request, CancellationToken ct = default);
        Task<bool> DeleteAsync(string fileUrl, CancellationToken ct = default);
        Task<string> GetPresignedUrlAsync(string fileUrl, int expiryMinutes = 60, CancellationToken ct = default);
        Task<bool> ExistsAsync(string fileUrl, CancellationToken ct = default);
        string GetPublicUrl(string filePath);
    }
}
