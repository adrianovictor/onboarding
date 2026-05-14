using Core.Common.Lib.Application.Results;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Core.Common.Lib.Api.Controllers.Interfaces;

internal interface IBaseController
{
    Task<IActionResult> ExecuteRequestAsync<TRequest, TResponse>(
        TRequest request,
        Func<TRequest, string>? buildInfoMessage,
        Func<TRequest, Exception, string>? buildErrorMessage,
        IValidator<TRequest>? validator,
        Func<TRequest, Task<TRequest>>? preProcessRequest,
        Func<TResponse, Task<TResponse>>? postProcessResponse,
        string actionName)
        where TRequest: IRequest<Result<TResponse>>;
}
