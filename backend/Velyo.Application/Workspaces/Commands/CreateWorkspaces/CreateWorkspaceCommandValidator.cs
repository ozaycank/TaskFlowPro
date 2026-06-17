using FluentValidation;

namespace Velyo.Application.Workspaces.Commands.CreateWorkspace;

public class CreateWorkspaceCommandValidator : AbstractValidator<CreateWorkspaceCommand>
{
    public CreateWorkspaceCommandValidator()
    {
        RuleFor(v => v.Name)
            .MaximumLength(200)
            .NotEmpty();

        RuleFor(v => v.Description)
            .MaximumLength(1000);
    }
}