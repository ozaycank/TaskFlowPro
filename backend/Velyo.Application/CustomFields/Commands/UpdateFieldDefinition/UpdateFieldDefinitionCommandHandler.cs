using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;
using Velyo.Domain.Enums;

namespace Velyo.Application.CustomFields.Commands.UpdateFieldDefinition;

public class UpdateFieldDefinitionCommandHandler : IRequestHandler<UpdateFieldDefinitionCommand>
{
    private readonly ICustomFieldDefinitionRepository _repository;
    private readonly IWorkspaceAuthorizationService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateFieldDefinitionCommandHandler(ICustomFieldDefinitionRepository repository, IWorkspaceAuthorizationService authService, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateFieldDefinitionCommand request, CancellationToken cancellationToken)
    {
        var field = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (field == null) throw new NotFoundException(nameof(CustomFieldDefinition), request.Id);

        await _authService.AuthorizeRoleAsync(field.WorkspaceId, WorkspaceRole.Admin, cancellationToken);

        // Map updates via reflection or direct assignment (Assuming direct assignment for simplicity)
        var propertyInfo = typeof(CustomFieldDefinition).GetProperty("Name");
        if (propertyInfo != null && propertyInfo.CanWrite) propertyInfo.SetValue(field, request.Name);

        propertyInfo = typeof(CustomFieldDefinition).GetProperty("OptionsJson");
        if (propertyInfo != null && propertyInfo.CanWrite) propertyInfo.SetValue(field, request.OptionsJson);

        propertyInfo = typeof(CustomFieldDefinition).GetProperty("IsRequired");
        if (propertyInfo != null && propertyInfo.CanWrite) propertyInfo.SetValue(field, request.IsRequired);

        // _repository.Update() is typically handled by EF Core change tracking, but if you have an explicit method:
        // _repository.Update(field); 

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}