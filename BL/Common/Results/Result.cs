using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Common.Results;

public class Result<T> : IResult<T>
{
    private readonly T? _value;

    private readonly List<Error> _errors = [];

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public T Value =>
        IsSuccess
            ? _value!
            : throw new InvalidOperationException("No value exists.");

    public IReadOnlyList<Error> Errors => _errors;

    public Error? FirstError => _errors.FirstOrDefault();

    private Result(T value)
    {
        IsSuccess = true;
        _value = value;
    }

    private Result(IEnumerable<Error> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        if (!errors.Any())
            throw new ArgumentException(
                "Failure must contain at least one error.");

        IsSuccess = false;
        _errors.AddRange(errors);
    }

    public static Result<T> Success(T value)
        => new(value);

    public static Result<T> Failure(params Error[] errors)
        => new(errors);

    // implicit operators

    public static implicit operator Result<T>(T value)
        => Success(value);

    public static implicit operator Result<T>(Error error)
        => Failure(error);

    public static implicit operator Result<T>(List<Error> errors)
        => new(errors);

}
