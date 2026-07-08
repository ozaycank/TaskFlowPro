using MediatR;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;

namespace Velyo.Application.CustomFields.Queries.GetFieldDefinitions;

public class GetFieldDefinitionsQueryHandler : IRequestHandler<GetFieldDefinitionsQuery, IEnumerable<CustomFieldDefinitionDto>>
{
    private readonly ICustomFieldDefinitionRepository _repository;
    private readonly IWorkspaceAuthorizationService _authService;

    public GetFieldDefinitionsQueryHandler(ICustomFieldDefinitionRepository repository, IWorkspaceAuthorizationService authService)
    {
        _repository = repository;
        _authService = authService;
    }

    public async Task<IEnumerable<CustomFieldDefinitionDto>> Handle(GetFieldDefinitionsQuery request, CancellationToken cancellationToken)
    {
        // SECURE: Tenant Isolation
        await _authService.AuthorizeMembershipAsync(request.WorkspaceId, cancellationToken);

        var definitions = await _repository.GetByWorkspaceIdAsync(request.WorkspaceId, cancellationToken);

        // Filter by project if requested, otherwise return workspace-level and project-level fields
        if (request.ProjectId.HasValue)
        {
            definitions = definitions.Where(d => d.ProjectId == null || d.ProjectId == request.ProjectId.Value);
        }

        return definitions.Select(d => new CustomFieldDefinitionDto(d.Id, d.WorkspaceId, d.ProjectId, d.Name, d.Type, d.OptionsJson, d.IsRequired));
    }
}