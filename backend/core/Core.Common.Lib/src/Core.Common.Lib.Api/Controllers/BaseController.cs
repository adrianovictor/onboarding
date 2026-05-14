using System.Net;
using System.Runtime.CompilerServices;
using Core.Common.Lib.Api.Controllers.Interfaces;
using Core.Common.Lib.Api.Responses;
using Core.Common.Lib.Application.Context;
using Core.Common.Lib.Application.Exceptions;
using Core.Common.Lib.Application.Logging;
using Core.Common.Lib.Application.Results;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core.Common.Lib.Api.Controllers;

public class BaseController<TController>(
    IMediator mediator, 
    ILogService<TController> logger, 
    IRequestContext requestContext) : ControllerBase, IBaseController
{
    protected string ControllerName => typeof(TController).Name;

    protected virtual async Task ValidationRequestAsync<T>(T request, IValidator<T> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            throw new CustomException(ErrorCodes.ValidationError, HttpStatusCode.BadRequest.ToString(), validationResult.ToString());
        }
    }

    protected IActionResult OkResponse<T>(T data) =>
        Ok(ApiResponse<T>.CreateSuccessResponse(data, requestContext));

    protected IActionResult OkResponseWithMessage<T>(T data, string message) =>
        Ok(ApiResponse<T>.CreateSuccessResponseWithMessage(data, message, requestContext));

    protected IActionResult CreateResponse<T>(T data, List<string>? warnings = null)
    {
        if (warnings?.Count > 0)
            return Created(string.Empty, ApiResponse<T>.CreateSuccessResponseWithWarnings(data, warnings, requestContext));

        return Created(string.Empty, ApiResponse<T>.CreateSuccessResponse(data, requestContext));   
    }
    
    protected IActionResult BadRequestResponse<T>(string errorMessage) =>
        BadRequest(ApiResponse<T>.CreateErrorResponse(errorMessage, requestContext));

    protected IActionResult BadRequestResponse<T>(List<string> errors) =>
        BadRequest(ApiResponse<T>.CreateErrorResponse(errors, requestContext));

    protected IActionResult NotFoundResponse<T>(string message = "Recurso não encontrado.") =>
        NotFound(ApiResponse<T>.CreateErrorResponse(message, requestContext));

    protected IActionResult NoContentResponse() => NoContent();

    /// <summary>401 Unauthorized.</summary>
    protected IActionResult UnauthorizedResponse<T>(string message) =>
        StatusCode(StatusCodes.Status401Unauthorized,
            ApiResponse<T>.CreateErrorResponse(message, requestContext));

    /// <summary>409 Conflict.</summary>
    protected IActionResult ConflictResponse<T>(string message) =>
        Conflict(ApiResponse<T>.CreateErrorResponse(message, requestContext));

    /// <summary>403 Forbidden.</summary>
    protected IActionResult ForbiddenResponse<T>(string message) =>
        StatusCode(StatusCodes.Status403Forbidden,
            ApiResponse<T>.CreateErrorResponse(message, requestContext));

    protected RequestProcessor<TRequest, TResponse> ProcessRequest<TRequest, TResponse>(
        TRequest request,
        [CallerMemberName] string actionName = "")
        where TRequest : IRequest<Result<TResponse>>
    {
        return new RequestProcessor<TRequest, TResponse>(this, request, actionName);
    }    

    public async Task<IActionResult> ExecuteRequestAsync<TRequest, TResponse>(
        TRequest request, 
        Func<TRequest, string>? buildInfoMessage, 
        Func<TRequest, Exception, string>? buildErrorMessage, 
        IValidator<TRequest>? validator, 
        Func<TRequest, Task<TRequest>>? preProcessRequest, 
        Func<TResponse, Task<TResponse>>? postProcessResponse, 
        string actionName) where TRequest : IRequest<Result<TResponse>>
    {
        try
        {
            var infoMEssage = buildInfoMessage?.Invoke(request) 
                ?? $"{ControllerName}.{actionName}: iniciando processamento da requisição.";

            logger.LogInfo(ControllerName, actionName, infoMEssage);

            if (preProcessRequest != null)
                request = await preProcessRequest(request);

            if (validator != null)
            {
                var validation = await validator.ValidateAsync(request);
                if (!validation.IsValid)
                {
                    var errors = validation.Errors.Select(_ => _.ErrorMessage).ToList();
                    return BadRequestResponse<TResponse>(errors);
                }
            }

            var result = await mediator.Send(request);
            if (!result.IsSuccess)
                return MapErrorToResponse<TResponse>(result.Error);

            var data = result.Value;
            if (postProcessResponse != null)
                data = await postProcessResponse(data);

            return Request.Method switch
            {
                var m when HttpMethods.IsPost(m) => CreateResponse(data),
                var m when HttpMethods.IsPatch(m) => OkResponseWithMessage(data, "Dados atualizados com sucesso."),
                _ => OkResponse(data)
            };

        }
        catch (Exception ex)
        {
            var errorMessage = buildErrorMessage?.Invoke(request, ex) 
                ?? $"{ControllerName}.{actionName}: erro ao processar requisição. Erro: {ex.Message}";
            
            logger.LogError(ControllerName, actionName, errorMessage, ex);
            
            return BadRequestResponse<TResponse>(errorMessage);;
        }
    }

    private IActionResult MapErrorToResponse<TResponse>(Error error) =>
        error.Type switch
        {
            ErrorType.NotFound => NotFoundResponse<TResponse>(error.Message),
            ErrorType.Validation => BadRequestResponse<TResponse>(error.Message),
            ErrorType.Unauthorized => UnauthorizedResponse<TResponse>(error.Message),
            ErrorType.Forbidden => ForbiddenResponse<TResponse>(error.Message),
            ErrorType.Conflict => ConflictResponse<TResponse>(error.Message),
            _ => BadRequestResponse<TResponse>(error.Message)
        };
}

