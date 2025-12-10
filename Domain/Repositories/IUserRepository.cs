using Domain.Entities.User;

namespace Domain.Repositories;

public interface IUserRepository : IRepository<User, int>
{
}