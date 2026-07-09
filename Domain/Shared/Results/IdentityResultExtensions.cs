using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shared.Results;

public static class IdentityResultExtensions
{
    public static List<Error> ToErrors(this IdentityResult result)
    {
        return result.Errors
            .Select(e => Error.Validation(
                e.Code,
                e.Description))
            .ToList();
    }

    public static Result ToResult(this IdentityResult result)
    {
        return result.Succeeded
            ? Result.Success()
            : Result.Failure(result.ToErrors());
    }

    public static Result<T> ToFailureResult<T>(this IdentityResult result)
    {
        return result.ToErrors();
    }
}