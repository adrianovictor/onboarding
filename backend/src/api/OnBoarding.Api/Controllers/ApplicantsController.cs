using Core.Common.Lib.Api.Controllers;
using Core.Common.Lib.Api.Responses;
using Core.Common.Lib.Application.Context;
using Core.Common.Lib.Application.Logging;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OnBoarding.Application.Applicants.Commands;

namespace OnBoarding.Api.Controllers;

[ApiController]
[Asp.Versioning.ApiVersion("1.0")]
[Route("v{version:apiVersion}/api/[controller]")]
[EnableCors("Angular")]
public class ApplicantsController(
    IMediator mediator, 
    IRequestContext requestContext, 
    ILogService<ApplicantsController> logService) : BaseController<ApplicantsController>(mediator, logService, requestContext)
{
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<RegisterApplicantResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register(
        [FromBody] RegisterApplicantCommand command, 
        [FromServices] IValidator<RegisterApplicantCommand> validator) =>
        await ProcessRequest<RegisterApplicantCommand, RegisterApplicantResponse>(command)
            .WithValidator(validator)
            .WithInfoMessage(r => $"Registrando o proponente {r.FullName} com CPF {r.Cpf}.")
            .WithErrorMessage((r, ex) => $"Erro ao registrar o proponente {r.FullName} com CPF {r.Cpf}. Detalhes: {ex.Message}")
            .ExecuteAsync();

}

