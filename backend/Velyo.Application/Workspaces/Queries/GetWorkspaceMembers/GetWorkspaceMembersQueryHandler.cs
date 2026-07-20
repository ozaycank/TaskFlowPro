using MediatR;
using Velyo.Application.Common.Interfaces.Repositories;

namespace Velyo.Application.Workspaces.Queries.GetWorkspaceMembers;

public class GetWorkspaceMembersQueryHandler : IRequestHandler<GetWorkspaceMembersQuery, List<WorkspaceMemberDto>>
{
    private readonly IWorkspaceMemberRepository _workspaceMemberRepository;
    private readonly IUserRepository _userRepository;

    public GetWorkspaceMembersQueryHandler(
        IWorkspaceMemberRepository workspaceMemberRepository,
        IUserRepository userRepository)
    {
        _workspaceMemberRepository = workspaceMemberRepository;
        _userRepository = userRepository;
    }

    public async Task<List<WorkspaceMemberDto>> Handle(GetWorkspaceMembersQuery request, CancellationToken cancellationToken)
    {
        var members = await _workspaceMemberRepository.GetByWorkspaceIdAsync(request.WorkspaceId, cancellationToken);

        var result = new List<WorkspaceMemberDto>();

        foreach (var member in members)
        {
            var user = await _userRepository.GetByIdAsync(member.UserId, cancellationToken);
            if (user != null)
            {
                result.Add(new WorkspaceMemberDto(
                    member.Id,
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    member.Role.ToString(),
                    member.CreatedAt
                ));
            }
        }

        return result;
    }
}