using Microsoft.AspNetCore.Http;

namespace BL.Common;

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }

    public static ApiResponse<T> Ok(T data) => new()
    {
        Success = true,
        StatusCode = StatusCodes.Status200OK,
        Data = data
    };

    public static ApiResponse<T> Created(T data) => new()
    {
        Success = true,
        StatusCode = StatusCodes.Status201Created,
        Data = data
    };
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