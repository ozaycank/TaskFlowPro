using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.CustomFields.Commands.SetTaskFieldValue;

// Komut Tanımı (Request)
public record SetTaskFieldValueCommand(Guid TaskId, Guid FieldDefinitionId, string Value) : IRequest;

public class SetTaskFieldValueCommandHandler : IRequestHandler<SetTaskFieldValueCommand>
{
    private readonly ITaskItemRepository _taskRepository;
    private readonly ICustomFieldDefinitionRepository _fieldDefinitionRepository;
    private readonly IWorkspaceAuthorizationService _authService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public SetTaskFieldValueCommandHandler(
        ITaskItemRepository taskRepository,
        ICustomFieldDefinitionRepository fieldDefinitionRepository,
        IWorkspaceAuthorizationService authService,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _taskRepository = taskRepository;
        _fieldDefinitionRepository = fieldDefinitionRepository;
        _authService = authService;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(SetTaskFieldValueCommand request, CancellationToken cancellationToken)
    {
        // 1. Görevi Bul
        var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
        if (task == null) throw new NotFoundException(nameof(TaskItem), request.TaskId);

        // 2. Güvenlik: Kullanıcının bu Workspace'te işlem yapma yetkisi var mı?
        await _authService.AuthorizeMembershipAsync(task.WorkspaceId, cancellationToken);

        // 3. Özel Alanın Varlığını Doğrula
        var fieldDef = await _fieldDefinitionRepository.GetByIdAsync(request.FieldDefinitionId, cancellationToken);
        if (fieldDef == null) throw new NotFoundException(nameof(CustomFieldDefinition), request.FieldDefinitionId);

        // 4. Güvenlik: Bu Özel Alan, bu görevle aynı Workspace veya Projeye mi ait?
        if (fieldDef.WorkspaceId != task.WorkspaceId)
            throw new InvalidOperationException("Field definition does not belong to this workspace.");

        // 5. JSONB Verisini Güncelle
        // TaskItem.cs içinde yazdığınız UpdateCustomField metodunu çağırıyoruz
        task.UpdateCustomField(request.FieldDefinitionId, request.Value);

        // Gelecekte TaskItem.UpdateCustomField içine şu event'i tetikleyecek kodu ekleyebilirsiniz:
        // AddDomainEvent(new TaskUpdatedEvent(this, Guid.Parse(_currentUserService.UserId!)));

        // 6. Kaydet (Outbox ve JSONB birlikte güncellenir)
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}