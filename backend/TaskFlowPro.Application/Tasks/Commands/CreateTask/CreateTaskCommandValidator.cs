using FluentValidation;

namespace TaskFlowPro.Application.Tasks.Commands.CreateTask;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(v => v.WorkspaceId).NotEmpty();
        RuleFor(v => v.ProjectId).NotEmpty();

        RuleFor(v => v.Title)
            .MaximumLength(300)
            .NotEmpty();

        RuleFor(v => v.Description)
            .MaximumLength(2000);

        RuleFor(v => v.Priority).IsInEnum();
    }
}