using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Tasks.Commands.UpdateTask;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand>
{
    private readonly ITaskItemRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWorkspaceAuthorizationService _authorizationService;

    public UpdateTaskCommandHandler(
        ITaskItemRepository taskRepository,
        IUnitOfWork unitOfWork,
        IWorkspaceAuthorizationService authorizationService)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
        _authorizationService = authorizationService;
    }

    public async Task Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
        if (task == null)
        {
            throw new NotFoundException(nameof(TaskItem), request.TaskId);
        }

        // 1. TENANT ISOLATION CHECK: Look up the workspace of the fetched task
        await _authorizationService.AuthorizeMembershipAsync(task.WorkspaceId, cancellationToken);

        // Domain method encapsulated logic
        task.UpdateDetails(request.Title, request.Description, request.Priority, request.DueDate);

        // YENİ: Alt Görev (ParentTask) bağlantısını kur
        // Domain entity'sinde direkt property setter yoksa Reflection veya Domain Metodu kullanabiliriz.
        // TaskItem.cs içinde ParentTaskId için bir method yazmadığımız için şimdilik property'i kullanabilirdik ama private set.
        // O yüzden TaskItem'ın UpdateDetails methoduna bunları eklemek yerine direkt Reflection ile veya 
        // daha kolayı, Entity Framework'ün izlediği (Tracked) nesne üzerinden Entity girişini güncelleyeceğiz.
        // Not: Temiz mimaride TaskItem.cs içine bir UpdateParentTask() ve ReplaceLabels() metodu koymak en doğrusudur. 
        // Şimdilik hızlı ilerlemek adına bu Handler'da Reflection kullanıyorum, siz Domain nesnesine method eklerseniz daha temiz olur.
        var parentProp = task.GetType().GetProperty("ParentTaskId");
        if (parentProp != null && parentProp.CanWrite)
        {
            parentProp.SetValue(task, request.ParentTaskId);
        }

        if (request.Labels != null)
        {
            var currentLabels = task.Labels.ToList();
            foreach (var label in currentLabels)
            {
                task.RemoveLabel(label);
            }
            foreach (var label in request.Labels)
            {
                task.AddLabel(label);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}