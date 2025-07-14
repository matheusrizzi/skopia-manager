using SkopiaManager.Domain.Entities;

namespace SkopiaManager.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
}
