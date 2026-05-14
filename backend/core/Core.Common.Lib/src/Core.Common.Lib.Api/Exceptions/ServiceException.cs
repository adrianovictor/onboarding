using Core.Common.Lib.Application.Exceptions;

namespace Core.Common.Lib.Api.Exceptions;

public class ServiceException : CustomException
{
    public ServiceException(string message)
        : this(message, innerException: null!) { }

    public ServiceException(string message, Exception innerException)
        : base(ErrorCodes.ServiceError, "Erro no serviço", message, innerException) { }
}
