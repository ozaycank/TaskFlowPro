using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Attachments.Queries.GetAttachmentsByTask;

public class GetAttachmentsByTaskQueryHandler : IRequestHandler<GetAttachmentsByTaskQuery, IEnumerable<AttachmentDto>>
{
    private readonly IAttachmentRepository _attachmentRepository;
    private readonly ITaskItemRepository _taskRepository;
    private readonly IWorkspaceAuthorizationService _authService;

    public GetAttachmentsByTaskQueryHandler(IAttachmentRepository attachmentRepository, ITaskItemRepository taskRepository, IWorkspaceAuthorizationService authService)
    {
        _attachmentRepository = attachmentRepository;
        _taskRepository = taskRepository;
        _authService = authService;
    }

    public async Task<IEnumerable<AttachmentDto>> Handle(GetAttachmentsByTaskQuery request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
        if (task == null) throw new NotFoundException(nameof(TaskItem), request.TaskId);

        await _authService.AuthorizeMembershipAsync(task.WorkspaceId, cancellationToken);

        var attachments = await _attachmentRepository.GetByTaskIdAsync(request.TaskId, cancellationToken);

        // Only return attachments that have successfully completed the S3 upload process
        return attachments.Where(a => a.IsUploaded).Select(a => new AttachmentDto(a.Id, a.TaskId, a.FileName, a.ContentType, a.FileSizeBytes, a.IsUploaded, a.CreatedAt));
    }
}