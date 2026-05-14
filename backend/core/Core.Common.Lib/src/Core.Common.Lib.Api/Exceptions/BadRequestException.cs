using Core.Common.Lib.Application.Exceptions;

namespace Core.Common.Lib.Api.Exceptions;

public class BadRequestException : CustomException
{
    public BadRequestException(string message)
        : this(message, innerException: null!) { }

    public BadRequestException(string message, Exception innerException)
        : base(ErrorCodes.BadRequestError, "Requisição inválida", message, innerException) { }
}
