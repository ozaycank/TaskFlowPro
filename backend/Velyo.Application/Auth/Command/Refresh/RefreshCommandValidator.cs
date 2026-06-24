using FluentValidation;

namespace Velyo.Application.Auth.Commands.Refresh;

public class RefreshCommandValidator : AbstractValidator<RefreshCommand>
{
    public RefreshCommandValidator()
    {
        RuleFor(v => v.RefreshToken).NotEmpty().WithMessage("Refresh token is required.");
    }
}