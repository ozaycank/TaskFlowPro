using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Velyo.Application.Common.Interfaces.Services;

namespace Velyo.Infrastructure.Storage;

public class S3StorageService : IStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public S3StorageService(IAmazonS3 s3Client, IConfiguration configuration)
    {
        _s3Client = s3Client;
        _bucketName = configuration["Storage:BucketName"] ?? throw new ArgumentNullException("Storage:BucketName missing");
    }

    public Task<string> GeneratePresignedUploadUrlAsync(string storageKey, string contentType, TimeSpan expiration)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = storageKey,
            Verb = HttpVerb.PUT,
            Expires = DateTime.UtcNow.Add(expiration),
            ContentType = contentType
        };

        return Task.FromResult(_s3Client.GetPreSignedURL(request));
    }

    public Task<string> GeneratePresignedDownloadUrlAsync(string storageKey, TimeSpan expiration)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = storageKey,
            Verb = HttpVerb.GET,
            Expires = DateTime.UtcNow.Add(expiration)
        };

        return Task.FromResult(_s3Client.GetPreSignedURL(request));
    }

    public async Task<bool> FileExistsAsync(string storageKey)
    {
        try
        {
            var response = await _s3Client.GetObjectMetadataAsync(_bucketName, storageKey);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
    }
}