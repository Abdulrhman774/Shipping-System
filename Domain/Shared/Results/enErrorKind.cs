namespace Domain.Shared.Results;

public enum enErrorKind
{
    Failure,
    Validation,
    NotFound,
    Conflict,
    Unauthorized,
    Forbidden,
    Unexpected
}