using Core.Common.Lib.Application.Exceptions;

namespace Core.Common.Lib.Application.Results;

public record Error(string Code, string Message, ErrorType Type = ErrorType.Failure)
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);
    public static readonly Error NullValue = new("NULL_VALUE", "Value cannot be null.", ErrorType.Failure);


    public static Error NotFound(string resource) =>
        new(ErrorCodes.NotFoundError, $"{resource} was not found.", ErrorType.NotFound);

    public static Error Conflict(string resource) =>
        new(ErrorCodes.ConflictError, $"{resource} already exists.", ErrorType.Conflict);

    public static Error Validation(string message) =>
        new(ErrorCodes.ValidationError, message, ErrorType.Validation);

    public static Error Unauthorized(string message = "Unauthorized access.") =>
        new(ErrorCodes.UnauthorizedError, message, ErrorType.Unauthorized);

    public static Error Forbidden(string message = "Access forbidden.") =>
        new(ErrorCodes.ForbiddenError, message, ErrorType.Forbidden);

}
