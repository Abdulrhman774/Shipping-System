using BL.Common;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

public abstract class ApiControllerBase : Controller
{
    protected IActionResult ApiResult<T>(ApiResponse<T> response)
    {
        return response.Success
            ? Ok(response)
            : StatusCode(response.Success ? 200 : 400, response);
    }
}