using FitnessOperationsApi.DTOs.Files;

namespace FitnessOperationsApi.Services;

public interface IFileUploadService
{
    Task<UploadFileResponse> UploadAsync(IFormFile file);
}
