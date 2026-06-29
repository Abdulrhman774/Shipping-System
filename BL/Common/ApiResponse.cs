using Microsoft.AspNetCore.Http;

namespace BL.Common;

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }

    private static ApiResponse<T> Create(
        bool success,
        int statusCode,
        string? error = null,
        T? data = default)
    {
        return new ApiResponse<T>
        {
            Success = success,
            StatusCode = statusCode,
            Error = error,
            Data = data
        };
    }

    public static ApiResponse<T> Ok(T data)
        => Create(true, 200, data: data);

    public static new ApiResponse<T> BadRequest(string error)
        => Create(false, 400, error);

    public static new ApiResponse<T> Unauthorized(string error)
        => Create(false, 401, error);

    public static new ApiResponse<T> Forbidden(string error)
        => Create(false, 403, error);

    public static new ApiResponse<T> NotFound(string error)
        => Create(false, 404, error);

    public static new ApiResponse<T> Conflict(string error)
        => Create(false, 409, error);

    public static new ApiResponse<T> InternalServerError(string error)
        => Create(false, 500, error);


}

public class ApiResponse
{
    public bool Success { get; set; }

    public int StatusCode { get; set; }

    public string? Error { get; set; }

    public static ApiResponse Succeded() => new()
    {
        Success = true,
        StatusCode = StatusCodes.Status200OK
    };

    public static ApiResponse Created() => new()
    {
        Success = true,
        StatusCode = StatusCodes.Status201Created
    };

    public static ApiResponse BadRequest(string error) => new()
    {
        Success = false,
        StatusCode = StatusCodes.Status400BadRequest,
        Error = error
    };

    public static ApiResponse Unauthorized(string error) => new()
    {
        Success = false,
        StatusCode = StatusCodes.Status401Unauthorized,
        Error = error
    };

    public static ApiResponse Forbidden(string error) => new()
    {
        Success = false,
        StatusCode = StatusCodes.Status403Forbidden,
        Error = error
    };

    public static ApiResponse NotFound(string error) => new()
    {
        Success = false,
        StatusCode = StatusCodes.Status404NotFound,
        Error = error
    };

    public static ApiResponse Conflict(string error) => new()
    {
        Success = false,
        StatusCode = StatusCodes.Status409Conflict,
        Error = error
    };

    public static ApiResponse InternalServerError(string error) => new()
    {
        Success = false,
        StatusCode = StatusCodes.Status500InternalServerError,
        Error = error
    };
}