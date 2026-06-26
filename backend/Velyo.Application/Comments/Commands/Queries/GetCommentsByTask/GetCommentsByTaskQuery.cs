using MediatR;

namespace Velyo.Application.Comments.Queries.GetCommentsByTask;

public record GetCommentsByTaskQuery(Guid TaskId) : IRequest<IEnumerable<CommentDto>>;