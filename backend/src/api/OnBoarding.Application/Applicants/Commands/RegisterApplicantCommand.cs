using Core.Common.Lib.Application.Results;
using MediatR;

namespace OnBoarding.Application.Applicants.Commands;

public record RegisterApplicantCommand(
    string FullName,
    string Cpf,
    string Email,
    string Phone,
    string MotherName,
    string BirthDate
) : IRequest<Result<RegisterApplicantResponse>>;

public record RegisterApplicantResponse(Guid ApplicantId, string Status);
