

using Horizon.Domain.Interfaces.Services.StorageServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Horizon.Infrastructure.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<LocalFileStorageService> _logger;

        public LocalFileStorageService(IConfiguration config, ILogger<LocalFileStorageService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<FileUploadResult> UploadAsync(FileUploadRequest request, CancellationToken ct = default)
        {
            try
            {
                var localPath = _config["Storage:LocalPath"] ?? "wwwroot/uploads";
                var folder = Path.Combine(localPath, request.Folder);
                Directory.CreateDirectory(folder);

                var ext = Path.GetExtension(request.FileName);
                var fileName = $"{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(folder, fileName);

                await using var stream = File.Create(filePath);
                await request.Content.CopyToAsync(stream, ct);

                var relativePath = Path.Combine(request.Folder, fileName).Replace('\\', '/');
                return new FileUploadResult
                {
                    Success = true,
                    FileUrl = GetPublicUrl(relativePath),
                    FilePath = relativePath,
                    FileName = fileName,
                    SizeBytes = new FileInfo(filePath).Length,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload file {FileName}", request.FileName);
                return new FileUploadResult { Success = false, Error = ex.Message };
            }
        }

        public Task<bool> DeleteAsync(string fileUrl, CancellationToken ct = default)
        {
            try
            {
                var localPath = _config["Storage:LocalPath"] ?? "wwwroot/uploads";
                var baseUrl = _config["Storage:BaseUrl"] ?? string.Empty;
                var relative = fileUrl.Replace(baseUrl, "").TrimStart('/');
                var fullPath = Path.Combine(localPath, relative);
                if (File.Exists(fullPath)) File.Delete(fullPath);
                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        public Task<string> GetPresignedUrlAsync(string fileUrl, int expiryMinutes = 60, CancellationToken ct = default)
            => Task.FromResult(fileUrl); // Local storage — URL is already public

        public Task<bool> ExistsAsync(string fileUrl, CancellationToken ct = default)
        {
            var localPath = _config["Storage:LocalPath"] ?? "wwwroot/uploads";
            var baseUrl = _config["Storage:BaseUrl"] ?? string.Empty;
            var relative = fileUrl.Replace(baseUrl, "").TrimStart('/');
            return Task.FromResult(File.Exists(Path.Combine(localPath, relative)));
        }

        public string GetPublicUrl(string filePath)
            => $"{_config["Storage:BaseUrl"]?.TrimEnd('/')}/{filePath.TrimStart('/')}";
    }
}
