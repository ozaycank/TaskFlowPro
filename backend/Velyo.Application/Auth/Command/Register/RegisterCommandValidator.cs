using FluentValidation;
namespace Velyo.Application.Auth.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(v => v.Email).NotEmpty().EmailAddress();
        RuleFor(v => v.Password).NotEmpty().MinimumLength(8);
        RuleFor(v => v.FirstName).NotEmpty();
        RuleFor(v => v.LastName).NotEmpty();
    }
}