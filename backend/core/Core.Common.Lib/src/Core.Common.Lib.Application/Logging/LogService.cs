using Core.Common.Lib.Application.Context;
using Microsoft.Extensions.Logging;

namespace Core.Common.Lib.Application.Logging;

public class LogService<T>(ILogger<T> logger, IRequestContext requestContext) : ILogService<T>
{
    public void LogInfo(string message) =>
        logger.LogInformation("[{CorrelationId}] {Message}", requestContext.CorrelationId, message);

    public void LogWarning(string message) =>
        logger.LogWarning("[{CorrelationId}] {Message}", requestContext.CorrelationId, message);

    public void LogError(string message, Exception? ex = null) =>
        logger.LogError("[{CorrelationId}] {Message} {Exception}", requestContext.CorrelationId, message, ex);

    public void LogInfo(string className, string methodName, string message) =>
        logger.LogInformation("[{CorrelationId}] {ClassName}.{MethodName} - {Message}", requestContext.CorrelationId, className, methodName, message);

    public void LogWarning(string className, string methodName, string message) =>
        logger.LogWarning("[{CorrelationId}] {ClassName}.{MethodName} - {Message}", requestContext.CorrelationId, className, methodName, message);

    public void LogError(string className, string methodName, string message, Exception? ex = null) =>
        logger.LogError("[{CorrelationId}] {ClassName}.{MethodName} - {Message} {Exception}", requestContext.CorrelationId, className, methodName, message, ex);

    public void LogStarting(string className, string methodName, object? parameters = null) =>
        logger.LogInformation("[{CorrelationId}] Starting {ClassName}.{MethodName} with parameters: {@Parameters}", requestContext.CorrelationId, className, methodName, parameters);

    public void LogCompleted(string className, string methodName) =>
        logger.LogInformation("[{CorrelationId}] Completed {ClassName}.{MethodName}", requestContext.CorrelationId, className, methodName);

    public void LogFailed(string className, string methodName, Exception ex) =>
        logger.LogError(ex, "[{CorrelationId}] Failed {ClassName}.{MethodName}", requestContext.CorrelationId, className, methodName);

    public void LogFailed(string className, string methodName, string message, Exception ex) =>
        logger.LogError(ex, "[{CorrelationId}] Failed {ClassName}.{MethodName} - {Message}", requestContext.CorrelationId, className, methodName, message);

}
