using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Attachments.Commands.CompleteUpload;

public record CompleteUploadCommand(Guid AttachmentId) : IRequest;

public class CompleteUploadCommandHandler : IRequestHandler<CompleteUploadCommand>
{
    private readonly IAttachmentRepository _attachmentRepository;
    private readonly IStorageService _storageService;
    private readonly IWorkspaceAuthorizationService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteUploadCommandHandler(IAttachmentRepository attachmentRepository, IStorageService storageService, IWorkspaceAuthorizationService authService, IUnitOfWork unitOfWork)
    {
        _attachmentRepository = attachmentRepository;
        _storageService = storageService;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CompleteUploadCommand request, CancellationToken cancellationToken)
    {
        var attachment = await _attachmentRepository.GetByIdAsync(request.AttachmentId, cancellationToken);
        if (attachment == null) throw new NotFoundException(nameof(Attachment), request.AttachmentId);

        // SECURE: Only workspace members can confirm uploads
        await _authService.AuthorizeMembershipAsync(attachment.WorkspaceId, cancellationToken);

        // SECURE: Verify that the file actually exists in S3 before marking it as uploaded
        var fileExists = await _storageService.FileExistsAsync(attachment.StorageKey);
        if (!fileExists) throw new InvalidOperationException("File not found in storage. Upload incomplete.");

        // Marks as true AND triggers AttachmentUploadedEvent for Outbox & SignalR
        attachment.MarkAsUploaded();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}