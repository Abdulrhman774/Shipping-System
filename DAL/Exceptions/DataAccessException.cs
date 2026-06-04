using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Exceptions;

public class DataAccessException : Exception
{
    public DataAccessException() : base("An error occurred while accessing the data store.")
    {
    }

    public DataAccessException(string message) : base(message)
    {
    }

    public DataAccessException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
