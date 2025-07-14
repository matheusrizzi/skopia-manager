using Microsoft.EntityFrameworkCore;
using SkopiaManager.Domain.Entities;
using SkopiaManager.Domain.Interfaces;
using SkopiaManager.Infrastructure.Data;

namespace SkopiaManager.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SkopiaDbContext _context;

    public UserRepository(SkopiaDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }
}
