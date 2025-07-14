using SkopiaManager.Domain.Entities;

namespace SkopiaManager.Domain.Interfaces;

public interface ICommentRepository
{
    Task AddAsync(Comment comment, CancellationToken cancellationToken);
}
