using Core.Common.Lib.Application.Exceptions;

namespace Core.Common.Lib.Api.Exceptions;

public class ConflictException : CustomException
{
    public ConflictException(string message)
        : this(message, innerException: null!) { }

    public ConflictException(string message, Exception innerException)
        : base(ErrorCodes.ConflictError, "Conflito de dados", message, innerException) { }
}
