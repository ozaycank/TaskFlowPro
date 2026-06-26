using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Comments.Queries.GetCommentsByTask;

public class GetCommentsByTaskQueryHandler : IRequestHandler<GetCommentsByTaskQuery, IEnumerable<CommentDto>>
{
    private readonly ICommentRepository _commentRepository;
    private readonly ITaskItemRepository _taskRepository;
    private readonly IWorkspaceAuthorizationService _authService;

    public GetCommentsByTaskQueryHandler(ICommentRepository commentRepository, ITaskItemRepository taskRepository, IWorkspaceAuthorizationService authService)
    {
        _commentRepository = commentRepository;
        _taskRepository = taskRepository;
        _authService = authService;
    }

    public async Task<IEnumerable<CommentDto>> Handle(GetCommentsByTaskQuery request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
        if (task == null) throw new NotFoundException(nameof(TaskItem), request.TaskId);

        // SECURE: Tenant isolation check
        await _authService.AuthorizeMembershipAsync(task.WorkspaceId, cancellationToken);

        var comments = await _commentRepository.GetByTaskIdAsync(request.TaskId, cancellationToken);

        // Map to DTO (In a real app, we'd join with Users table to get FirstName/LastName)
        return comments.Select(c => new CommentDto(c.Id, c.TaskId, c.UserId, c.Content, c.IsEdited, c.CreatedAt, c.CreatedBy));
    }
}