using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Common.Results;

public interface IResult
{
    bool IsSuccess { get; }

    IReadOnlyList<Error> Errors { get; }
}

public interface IResult<out T> : IResult
{
    T Value { get; }
}