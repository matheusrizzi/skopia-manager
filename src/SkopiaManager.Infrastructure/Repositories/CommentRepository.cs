using SkopiaManager.Domain.Entities;
using SkopiaManager.Domain.Interfaces;
using SkopiaManager.Infrastructure.Data;

namespace SkopiaManager.Infrastructure.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly SkopiaDbContext _context;

    public CommentRepository(SkopiaDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Comment comment, CancellationToken cancellationToken)
    {
        await _context.Comments.AddAsync(comment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
