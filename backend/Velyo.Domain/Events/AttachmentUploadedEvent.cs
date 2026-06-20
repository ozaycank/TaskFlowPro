using Velyo.Domain.Common.Models;
using Velyo.Domain.Entities;

namespace Velyo.Domain.Events;

public class AttachmentUploadedEvent : DomainEvent
{
    public Attachment Attachment { get; }
    public AttachmentUploadedEvent(Attachment attachment) => Attachment = attachment;
}