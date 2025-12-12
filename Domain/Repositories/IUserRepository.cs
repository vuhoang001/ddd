using Domain.Entities.User;

namespace Domain.Repositories;

public interface IUserRepository : IRepository<User, int>
{
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
}