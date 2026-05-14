using Core.Common.Lib.Application.Logging;
using MediatR;

namespace Core.Common.Lib.Api.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(
    ILogService<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        var name = typeof(TRequest).Name;

        logger.LogStarting(name, nameof(Handle), request);

        var response = await next(ct);

        logger.LogCompleted(name, nameof(Handle));

        return response;
    }
}
