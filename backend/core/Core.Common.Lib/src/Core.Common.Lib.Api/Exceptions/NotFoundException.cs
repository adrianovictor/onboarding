using Core.Common.Lib.Application.Exceptions;

namespace Core.Common.Lib.Api.Exceptions;

public class NotFoundException : CustomException
{
    public NotFoundException(string message)
        : this(message, innerException: null!) { }

    public NotFoundException(string message, Exception innerException)
        : base(ErrorCodes.NotFoundError, "Recurso não encontrado", message, innerException) { }
}

