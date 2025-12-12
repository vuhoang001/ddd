using Domain.Entities.User;
using Domain.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class UserSessionRepository : Repository<UserSession, int>, IUserSessionRepository
{
    public UserSessionRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}