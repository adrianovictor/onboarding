using System.Runtime.CompilerServices;
using Core.Common.Lib.Api.Controllers.Interfaces;
using Core.Common.Lib.Application.Results;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Core.Common.Lib.Api.Controllers;

public class RequestProcessor<TRequest, TResponse>
    where TRequest : IRequest<Result<TResponse>>
{
    //private readonly BaseController _controller;
    private readonly IBaseController _controller;
    private TRequest _request;
    private readonly string _actionName;

    private Func<TRequest, string>? _infoMessageBuilder;
    private Func<TRequest, Exception, string>? _errorMessageBuilder;
    private IValidator<TRequest>? _validator;
    private Func<TRequest, Task<TRequest>>? _preProcessor;
    private Func<TResponse, Task<TResponse>>? _postProcessor;

    internal RequestProcessor(IBaseController controller, TRequest request, string actionName)
    {
        _controller = controller;
        _request = request;
        _actionName = actionName;
    }

    public RequestProcessor<TRequest, TResponse> WithInfoMessage(Func<TRequest, string> builder)
    {
        _infoMessageBuilder = builder;
        return this;
    }

    public RequestProcessor<TRequest, TResponse> WithErrorMessage(Func<TRequest, Exception, string> builder)
    {
        _errorMessageBuilder = builder;
        return this;
    }

    public RequestProcessor<TRequest, TResponse> WithValidator(IValidator<TRequest> validator)
    {
        _validator = validator;
        return this;
    }

    public RequestProcessor<TRequest, TResponse> WithPreProcessor(Func<TRequest, Task<TRequest>> processor)
    {
        _preProcessor = processor;
        return this;
    }

    public RequestProcessor<TRequest, TResponse> WithPostProcessor(Func<TResponse, Task<TResponse>> processor)
    {
        _postProcessor = processor;
        return this;
    }

    public Task<IActionResult> ExecuteAsync() =>
        _controller.ExecuteRequestAsync(
            _request,
            _infoMessageBuilder,
            _errorMessageBuilder,
            _validator,
            _preProcessor,
            _postProcessor,
            _actionName
        );

    // Permite await direto sem .ExecuteAsync()
    public TaskAwaiter<IActionResult> GetAwaiter() => ExecuteAsync().GetAwaiter();
}

