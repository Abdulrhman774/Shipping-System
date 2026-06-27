using BL.Common;
using BL.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result, ControllerBase controller)
    {
        if (result.IsSuccess)
        {
            return controller.Ok(
                ApiResponse<T>.Ok(result.Value));
        }

        var error = result.FirstError;

        var response = error?.Type switch
        {
            enErrorKind.Validation =>
                ApiResponse<T>.BadRequest(error.Description),

            enErrorKind.NotFound =>
                ApiResponse<T>.NotFound(error.Description),

            enErrorKind.Unauthorized =>
                ApiResponse<T>.Unauthorized(error.Description),

            enErrorKind.Forbidden =>
                ApiResponse<T>.Forbidden(error.Description),

            enErrorKind.Conflict =>
                ApiResponse<T>.Conflict(error.Description),
   
            _ =>
                ApiResponse<T>.InternalServerError(error?.Description ?? "Server side error"),
        };

        return controller.StatusCode(response.StatusCode, response);
    }

    public static IActionResult ToActionResult(
    this Result result,
    ControllerBase controller)
    {
        if (result.IsSuccess)
        {
            return controller.Ok(
                ApiResponse.Succeded());
        }

        var error = result.Errors.First();

        var response = error.Type switch
        {
            enErrorKind.Validation =>
                ApiResponse.BadRequest(error.Description),

            enErrorKind.NotFound =>
                ApiResponse.NotFound(error.Description),

            enErrorKind.Unauthorized =>
                ApiResponse.Unauthorized(error.Description),

            enErrorKind.Forbidden =>
                ApiResponse.Forbidden(error.Description),

            enErrorKind.Conflict =>
                ApiResponse.Conflict(error.Description),

            _ =>
                ApiResponse.InternalServerError(
                    error.Description)
        };

        return controller.StatusCode(response.StatusCode, response);
    }
}