using Core.Common.Lib.Application.Logging;
using Core.Common.Lib.Application.Results;
using MediatR;

namespace OnBoarding.Application.Applicants.Commands;

public class RegisterApplicantHandler(
    ILogService<RegisterApplicantHandler> logger)
    : IRequestHandler<RegisterApplicantCommand, Result<RegisterApplicantResponse>>
{
    public Task<Result<RegisterApplicantResponse>> Handle(RegisterApplicantCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
