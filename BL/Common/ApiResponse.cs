namespace BL.Common;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public T? Data { get; set; }
    public string? Error { get; set; }

    public static ApiResponse<T> Ok(T data) => new()
    {
        Success = true,
        StatusCode = 200,
        Data = data
    };

    public static ApiResponse<T> Created(T data) => new()
    {
        Success = true,
        StatusCode = 201,
        Data = data
    };

    public static ApiResponse<T> NotFound(string error) => new()
    {
        Success = false,
        StatusCode = 404,
        Error = error
    };

    public static ApiResponse<T> BadRequest(string error) => new()
    {
        Success = false,
        StatusCode = 400,
        Error = error
    };

    public static ApiResponse<T> Unauthorized(string error) => new()
    {
        Success = false,
        StatusCode = 401,
        Error = error
    };

    public static ApiResponse<T> Forbidden(string error) => new()
    {
        Success = false,
        StatusCode = 403,
        Error = error
    };
}
