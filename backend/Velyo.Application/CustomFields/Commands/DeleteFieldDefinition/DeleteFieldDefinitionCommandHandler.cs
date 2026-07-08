using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;
using Velyo.Domain.Enums;

namespace Velyo.Application.CustomFields.Commands.DeleteFieldDefinition;

public class DeleteFieldDefinitionCommandHandler : IRequestHandler<DeleteFieldDefinitionCommand>
{
    private readonly ICustomFieldDefinitionRepository _repository;
    private readonly IWorkspaceAuthorizationService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteFieldDefinitionCommandHandler(ICustomFieldDefinitionRepository repository, IWorkspaceAuthorizationService authService, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteFieldDefinitionCommand request, CancellationToken cancellationToken)
    {
        var field = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (field == null) throw new NotFoundException(nameof(CustomFieldDefinition), request.Id);

        await _authService.AuthorizeRoleAsync(field.WorkspaceId, WorkspaceRole.Admin, cancellationToken);

        _repository.Delete(field);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}