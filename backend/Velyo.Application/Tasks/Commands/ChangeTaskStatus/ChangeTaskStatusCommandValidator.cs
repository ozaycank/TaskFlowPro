using FluentValidation;

namespace Velyo.Application.Tasks.Commands.ChangeTaskStatus;

public class ChangeTaskStatusCommandValidator : AbstractValidator<ChangeTaskStatusCommand>
{
    public ChangeTaskStatusCommandValidator()
    {
        RuleFor(v => v.TaskId).NotEmpty();
        RuleFor(v => v.NewStatus).IsInEnum();
    }
}