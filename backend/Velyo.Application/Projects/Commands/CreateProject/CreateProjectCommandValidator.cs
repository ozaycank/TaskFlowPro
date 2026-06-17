using FluentValidation;

namespace Velyo.Application.Projects.Commands.CreateProject;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(v => v.WorkspaceId)
            .NotEmpty().WithMessage("WorkspaceId is required.");

        RuleFor(v => v.Name)
            .MaximumLength(200)
            .NotEmpty();

        RuleFor(v => v.Description)
            .MaximumLength(1000);
    }
}