namespace FitnessOperationsApi.DTOs.Files;

public class UploadFileRequest
{
    public IFormFile File { get; set; } = default!;
}
