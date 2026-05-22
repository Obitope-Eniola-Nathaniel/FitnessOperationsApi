namespace FitnessOperationsApi.Common;

public class ApiResponse<T>
{
    public string Message { get; set; } = default!;
    public string ResponseCode { get; set; } = default!;
    public T? Data { get; set; }
    public DateTime ResponseTime { get; set; } = DateTime.UtcNow;

    public static ApiResponse<T> Success(T data, string message = "Request successful")
    {
        return new ApiResponse<T>
        {
            Message = message,
            ResponseCode = "00",
            Data = data
        };
    }

    public static ApiResponse<T> Failure(string message, string responseCode = "99")
    {
        return new ApiResponse<T>
        {
            Message = message,
            ResponseCode = responseCode,
            Data = default
        };
    }
}
