using System;
using System.Net;
using Core.Common.Lib.Api.Exceptions;
using Core.Common.Lib.Api.Responses;
using Core.Common.Lib.Application.Context;
using Core.Common.Lib.Application.Logging;
using Microsoft.AspNetCore.Http;

namespace Core.Common.Lib.Api.Middlewares;

public class GlobalExceptionMiddleware(ILogService<GlobalExceptionMiddleware> logger, IRequestContext requestContext) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogFailed(nameof(GlobalExceptionMiddleware), nameof(InvokeAsync), $"Erro interno. CorrelationId: {requestContext.CorrelationId}", ex);
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            NotFoundException => HttpStatusCode.NotFound,
            UnauthorizedException => HttpStatusCode.Unauthorized,
            ForbiddenException => HttpStatusCode.Forbidden,
            BadRequestException => HttpStatusCode.BadRequest,
            ConflictException => HttpStatusCode.Conflict,
            _ => HttpStatusCode.InternalServerError
        };

        if (context.Response.HasStarted)
        {
            logger.LogError($"Não foi possível manipular a exceção porque a resposta já foi iniciada. CorrelationId: {requestContext.CorrelationId}", exception);
            return Task.CompletedTask;
        }

        context.Response.Headers.TryAdd("CorrelationId", requestContext.CorrelationId);
        context.Response.Headers.TryAdd("Content-Type", "application/json");
        context.Response.StatusCode = (int)statusCode;

        var response = ApiResponse<object>.CreateErrorResponse(exception.Message, requestContext);
        
        return context.Response.WriteAsJsonAsync(response);
    }
}
