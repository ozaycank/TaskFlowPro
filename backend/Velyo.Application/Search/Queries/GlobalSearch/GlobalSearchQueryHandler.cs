using MediatR;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Search.Queries.GlobalSearch;

public record GlobalSearchQuery(Guid WorkspaceId, string SearchTerm) : IRequest<IEnumerable<SearchResultDto>>;
public record SearchResultDto(string EntityType, string Title, string Snippet, string Url);

public class GlobalSearchQueryHandler : IRequestHandler<GlobalSearchQuery, IEnumerable<SearchResultDto>>
{
    private readonly ISearchService _searchService;
    private readonly IWorkspaceAuthorizationService _authService;

    public GlobalSearchQueryHandler(ISearchService searchService, IWorkspaceAuthorizationService authService)
    {
        _searchService = searchService;
        _authService = authService;
    }

    public async Task<IEnumerable<SearchResultDto>> Handle(GlobalSearchQuery request, CancellationToken cancellationToken)
    {
        // SECURE: Tenant Isolation (Sadece üye olduğu Workspace'de arama yapabilir)
        await _authService.AuthorizeMembershipAsync(request.WorkspaceId, cancellationToken);

        var results = await _searchService.SearchWorkspaceAsync(request.WorkspaceId, request.SearchTerm, 10, cancellationToken);

        return results.Select(r => new SearchResultDto(
            r.EntityType,
            r.Title,
            r.Content.Length > 100 ? r.Content.Substring(0, 97) + "..." : r.Content, // Snippet
            r.Url
        ));
    }
}