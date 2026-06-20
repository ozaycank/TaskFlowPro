using Microsoft.Extensions.Logging;
using Velyo.Application.Common.Interfaces.Services;

namespace Velyo.Infrastructure.Storage;

public class MockStorageService : IStorageService
{
    private readonly ILogger<MockStorageService> _logger;

    public MockStorageService(ILogger<MockStorageService> logger)
    {
        _logger = logger;
    }

    public Task<string> GeneratePresignedUploadUrlAsync(string storageKey, string contentType, TimeSpan expiration)
    {
        _logger.LogInformation("========== MOCK S3 ==========");
        _logger.LogInformation("Generated UPLOAD URL for: {StorageKey}", storageKey);
        _logger.LogInformation("=============================");

        return Task.FromResult($"https://mock-s3.velyo.local/upload?key={storageKey}");
    }

    public Task<string> GeneratePresignedDownloadUrlAsync(string storageKey, TimeSpan expiration)
    {
        _logger.LogInformation("========== MOCK S3 ==========");
        _logger.LogInformation("Generated DOWNLOAD URL for: {StorageKey}", storageKey);
        _logger.LogInformation("=============================");

        return Task.FromResult($"https://mock-s3.velyo.local/download?key={storageKey}");
    }

    public Task<bool> FileExistsAsync(string storageKey)
    {
        _logger.LogInformation("MOCK S3: Verified file exists for {StorageKey}", storageKey);

        // Geliştirme ortamında dosya her zaman başarıyla S3'e yüklenmiş gibi davran
        return Task.FromResult(true);
    }
}