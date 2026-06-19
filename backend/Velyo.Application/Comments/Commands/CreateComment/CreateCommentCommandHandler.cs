using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Comments.Commands.CreateComment;

public record CreateCommentCommand(Guid TaskId, string Content) : IRequest<Guid>;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Guid>
{
    private readonly ICommentRepository _commentRepository;
    private readonly ITaskItemRepository _taskRepository;
    private readonly IWorkspaceAuthorizationService _authService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCommentCommandHandler(
        ICommentRepository commentRepository,
        ITaskItemRepository taskRepository,
        IWorkspaceAuthorizationService authService,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _commentRepository = commentRepository;
        _taskRepository = taskRepository;
        _authService = authService;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
        if (task == null) throw new NotFoundException(nameof(TaskItem), request.TaskId);

        // TENANT ISOLATION CHECK
        await _authService.AuthorizeMembershipAsync(task.WorkspaceId, cancellationToken);

        var userId = Guid.Parse(_currentUserService.UserId!);

        // Create comment and trigger Domain Event for SignalR/Notifications
        var comment = Comment.Create(task.Id, userId, request.Content, task.WorkspaceId);

        _commentRepository.Add(comment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return comment.Id;
    }
}