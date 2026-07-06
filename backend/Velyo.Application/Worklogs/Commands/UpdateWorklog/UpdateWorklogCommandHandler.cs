using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Worklogs.Commands.UpdateWorklog;

public class UpdateWorklogCommandHandler : IRequestHandler<UpdateWorklogCommand>
{
    private readonly IWorklogRepository _worklogRepository;
    private readonly IWorkspaceAuthorizationService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateWorklogCommandHandler(IWorklogRepository worklogRepository, IWorkspaceAuthorizationService authService, IUnitOfWork unitOfWork)
    {
        _worklogRepository = worklogRepository;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateWorklogCommand request, CancellationToken cancellationToken)
    {
        var worklog = await _worklogRepository.GetByIdAsync(request.WorklogId, cancellationToken);
        if (worklog == null) throw new NotFoundException(nameof(Worklog), request.WorklogId);

        // SECURE: Ensure user has access to the workspace this worklog belongs to
        await _authService.AuthorizeMembershipAsync(worklog.WorkspaceId, cancellationToken);

        worklog.Update(request.TimeSpentSeconds, request.StartDate, request.Description);

        _worklogRepository.Update(worklog);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}