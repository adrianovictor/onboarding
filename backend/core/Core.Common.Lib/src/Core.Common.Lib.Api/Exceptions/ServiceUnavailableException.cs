using Core.Common.Lib.Application.Exceptions;

namespace Core.Common.Lib.Api.Exceptions;

public class ServiceUnavailableException : CustomException
{
    public ServiceUnavailableException(string message)
        : this(message, innerException: null!) { }

    public ServiceUnavailableException(string message, Exception innerException)
        : base(ErrorCodes.ServiceUnavailableError, "Serviço indisponível", message, innerException) { }
}
