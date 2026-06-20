using MediatR;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;
using Velyo.Domain.Enums;

namespace Velyo.Application.CustomFields.Commands.CreateFieldDefinition;

public record CreateFieldDefinitionCommand(Guid WorkspaceId, Guid? ProjectId, string Name, FieldType Type, string? OptionsJson, bool IsRequired) : IRequest<Guid>;

public class CreateFieldDefinitionCommandHandler : IRequestHandler<CreateFieldDefinitionCommand, Guid>
{
    private readonly ICustomFieldDefinitionRepository _repository;
    private readonly IWorkspaceAuthorizationService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateFieldDefinitionCommandHandler(
        ICustomFieldDefinitionRepository repository,
        IWorkspaceAuthorizationService authService,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateFieldDefinitionCommand request, CancellationToken cancellationToken)
    {
        // SECURE: Tenant Isolation. Only Workspace Admins can define fields
        await _authService.AuthorizeRoleAsync(request.WorkspaceId, WorkspaceRole.Admin, cancellationToken);

        var definition = CustomFieldDefinition.Create(
            request.WorkspaceId,
            request.ProjectId,
            request.Name,
            request.Type,
            request.OptionsJson,
            request.IsRequired);

        _repository.Add(definition);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Future: Trigger Domain Event -> SignalR to update UI settings dynamically
        return definition.Id;
    }
}