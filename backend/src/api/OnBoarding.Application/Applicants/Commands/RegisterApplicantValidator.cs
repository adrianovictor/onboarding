using FluentValidation;

namespace OnBoarding.Application.Applicants.Commands;

public class RegisterApplicantValidator : AbstractValidator<RegisterApplicantCommand>
{
    public RegisterApplicantValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(200);

        RuleFor(x => x.Cpf)
            .NotEmpty().WithMessage("CPF is required.")
            .Length(11, 14).WithMessage("CPF must be valid.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be valid.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required.");

        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("Birth date is required.");
    }
}
