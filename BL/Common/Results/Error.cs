using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Common.Results;

public sealed record Error(
    string Code,
    string Description,
    enErrorKind Type)
{
    public static Error Failure(
        string code,
        string description)
        => new(code, description, enErrorKind.Failure);

    public static Error Validation(
        string code,
        string description)
        => new(code, description, enErrorKind.Validation);

    public static Error NotFound(
        string code,
        string description)
        => new(code, description, enErrorKind.NotFound);

    public static Error Conflict(
        string code,
        string description)
        => new(code, description, enErrorKind.Conflict);

    public static Error Unauthorized(
        string code,
        string description)
        => new(code, description, enErrorKind.Unauthorized);

    public static Error Forbidden(
        string code,
        string description)
        => new(code, description, enErrorKind.Forbidden);

    public static Error Unexpected(
        string code,
        string description)
        => new(code, description, enErrorKind.Unexpected);
}