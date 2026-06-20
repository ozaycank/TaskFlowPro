namespace Velyo.Application.Common.Interfaces.Services;

public interface IStorageService
{
    // Frontend'in doğrudan S3'e PUT isteği atabilmesi için verilen 15 dakikalık geçici link
    Task<string> GeneratePresignedUploadUrlAsync(string storageKey, string contentType, TimeSpan expiration);

    // Frontend'in S3'ten doğrudan dosya okuyabilmesi/indirebilmesi için geçici link
    Task<string> GeneratePresignedDownloadUrlAsync(string storageKey, TimeSpan expiration);

    // Yükleme tamamlandıktan sonra dosyanın gerçekten S3'e ulaşıp ulaşmadığını kontrol etme (Security)
    Task<bool> FileExistsAsync(string storageKey);
}