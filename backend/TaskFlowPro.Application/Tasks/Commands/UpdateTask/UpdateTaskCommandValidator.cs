using FluentValidation;

namespace TaskFlowPro.Application.Tasks.Commands.UpdateTask;

public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskCommandValidator()
    {
        RuleFor(v => v.TaskId).NotEmpty();

        RuleFor(v => v.Title)
            .MaximumLength(300)
            .NotEmpty();

        RuleFor(v => v.Description)
            .MaximumLength(2000);

        RuleFor(v => v.Priority).IsInEnum();
    }
}