using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Worklogs.Commands.DeleteWorklog;

public class DeleteWorklogCommandHandler : IRequestHandler<DeleteWorklogCommand>
{
    private readonly IWorklogRepository _worklogRepository;
    private readonly IWorkspaceAuthorizationService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteWorklogCommandHandler(IWorklogRepository worklogRepository, IWorkspaceAuthorizationService authService, IUnitOfWork unitOfWork)
    {
        _worklogRepository = worklogRepository;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteWorklogCommand request, CancellationToken cancellationToken)
    {
        var worklog = await _worklogRepository.GetByIdAsync(request.WorklogId, cancellationToken);
        if (worklog == null) throw new NotFoundException(nameof(Worklog), request.WorklogId);

        await _authService.AuthorizeMembershipAsync(worklog.WorkspaceId, cancellationToken);

        _worklogRepository.Delete(worklog);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}