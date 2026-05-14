using Core.Common.Lib.Application.Exceptions;

namespace Core.Common.Lib.Api.Exceptions;

public class UnauthorizedException : CustomException
{
    public UnauthorizedException(string message)
        : this(message, innerException: null!) { }

    public UnauthorizedException(string message, Exception innerException)
        : base(ErrorCodes.UnauthorizedError, "Acesso não autorizado", message, innerException) { }
}
