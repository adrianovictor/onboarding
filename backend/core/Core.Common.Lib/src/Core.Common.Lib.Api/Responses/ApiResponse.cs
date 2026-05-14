using Core.Common.Lib.Application.Context;

namespace Core.Common.Lib.Api.Responses;

public class ApiResponse<T>
{
    public bool Success { get; private set; }
    public string CorrelationId { get; private set; }
    public string? UserId { get; private set; }
    public T? Data { get; private set; }
    public List<string>? Errors { get; private set; }
    public List<string>? Warnings { get; private set; }
    public DateTime Timestamp { get; private set; }
    public string? Message { get; private set; }

    private ApiResponse(bool success, string correlationId, string? userId, T? data, List<string>? errors = null, List<string>? warnings = null, string? message = null)
    {
        Success = success;
        CorrelationId = correlationId;
        UserId = userId;
        Timestamp = DateTime.UtcNow;
        Data = data;
        Errors = errors;
        Warnings = warnings;
        Message = message;
    }

    public static ApiResponse<T> CreateSuccessResponse(T data, IRequestContext context) =>
        new(true, context.CorrelationId, context.UserId, data);

    public static ApiResponse<T> CreateSuccessResponseWithWarnings(T data, IEnumerable<string> warnings, IRequestContext context) =>
        new(true, context.CorrelationId, context.UserId, data, warnings: [.. warnings]);

    public static ApiResponse<T> CreateErrorResponse(string errorMessage, IRequestContext context) =>
        new(false, context.CorrelationId, context.UserId, default, errors: [errorMessage]);

    public static ApiResponse<T> CreateErrorResponse(IEnumerable<string> errors, IRequestContext context) =>
        new(false, context.CorrelationId, context.UserId, default, errors: [.. errors]);

    public static ApiResponse<T> CreateSuccessResponseWithMessage(T data, string message, IRequestContext context) =>
        new(true, context.CorrelationId, context.UserId, data, message: message);
}

