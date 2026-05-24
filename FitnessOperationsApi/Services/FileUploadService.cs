using FitnessOperationsApi.Configuration;
using FitnessOperationsApi.DTOs.Files;
using Microsoft.Extensions.Options;
using Npgsql.BackendMessages;
using System.Security.Principal;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace FitnessOperationsApi.Services;

public class FileUploadService : IFileUploadService
{
    private readonly Cloudinary _cloudinary;

    public FileUploadService(IOptions<CloudinarySettings> config)
    {
        var account = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );

        _cloudinary = new Cloudinary(account);
    }

    public async Task<UploadFileResponse> UploadAsync(
        IFormFile file)
    {
        if (file == null || file.Length == 0) throw new Exception("File is empty");

        await using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName,stream)
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        if (result.Error != null) throw new Exception(result.Error.Message);

        return new UploadFileResponse
        {
            Url = result.SecureUrl.ToString(),
            PublicId = result.PublicId
        };
    }
}
