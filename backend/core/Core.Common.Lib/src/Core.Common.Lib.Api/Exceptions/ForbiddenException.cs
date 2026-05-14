using Core.Common.Lib.Application.Exceptions;

namespace Core.Common.Lib.Api.Exceptions;

public class ForbiddenException : CustomException
{
    public ForbiddenException(string message)
        : this(message, innerException: null!) { }

    public ForbiddenException(string message, Exception innerException)
        : base(ErrorCodes.ForbiddenError, "Acesso proibido", message, innerException) { }
}

