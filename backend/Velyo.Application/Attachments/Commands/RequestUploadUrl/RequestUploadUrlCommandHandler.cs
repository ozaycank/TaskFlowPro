using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Attachments.Commands.RequestUploadUrl;

public record RequestUploadUrlCommand(Guid TaskId, Guid? CommentId, string FileName, string ContentType, long FileSizeBytes) : IRequest<UploadUrlResponseDto>;
public record UploadUrlResponseDto(Guid AttachmentId, string PresignedUrl);

public class RequestUploadUrlCommandHandler : IRequestHandler<RequestUploadUrlCommand, UploadUrlResponseDto>
{
    private readonly IAttachmentRepository _attachmentRepository;
    private readonly ITaskItemRepository _taskRepository;
    private readonly IWorkspaceAuthorizationService _authService;
    private readonly IStorageService _storageService;
    private readonly IUnitOfWork _unitOfWork;

    public RequestUploadUrlCommandHandler(
        IAttachmentRepository attachmentRepository,
        ITaskItemRepository taskRepository,
        IWorkspaceAuthorizationService authService,
        IStorageService storageService,
        IUnitOfWork unitOfWork)
    {
        _attachmentRepository = attachmentRepository;
        _taskRepository = taskRepository;
        _authService = authService;
        _storageService = storageService;
        _unitOfWork = unitOfWork;
    }

    public async Task<UploadUrlResponseDto> Handle(RequestUploadUrlCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate Task Existence & Tenant Isolation
        var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
        if (task == null) throw new NotFoundException(nameof(TaskItem), request.TaskId);
        await _authService.AuthorizeMembershipAsync(task.WorkspaceId, cancellationToken);

        // 2. Validate File Size (Enterprise SaaS rule: e.g., max 50MB)
        if (request.FileSizeBytes > 50 * 1024 * 1024) throw new InvalidOperationException("File size exceeds 50MB limit.");

        // 3. Create Pending Attachment
        var attachment = Attachment.CreatePending(task.WorkspaceId, task.Id, request.CommentId, request.FileName, request.ContentType, request.FileSizeBytes);
        _attachmentRepository.Add(attachment);

        // 4. Generate AWS S3 Presigned URL (Valid for 15 minutes)
        var presignedUrl = await _storageService.GeneratePresignedUploadUrlAsync(attachment.StorageKey, request.ContentType, TimeSpan.FromMinutes(15));

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UploadUrlResponseDto(attachment.Id, presignedUrl);
    }
}