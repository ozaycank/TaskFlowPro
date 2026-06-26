using MediatR;

namespace Velyo.Application.Attachments.Queries.GetAttachmentsByTask;

public record GetAttachmentsByTaskQuery(Guid TaskId) : IRequest<IEnumerable<AttachmentDto>>;