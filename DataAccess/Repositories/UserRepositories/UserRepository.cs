using Core.Attributes;
using DataAccess.DataContext;
using DataAccess.Entities;
using DataAccess.Repositories.BaseRepository;

namespace DataAccess.Repositories.UserRepositories;
[Injectable]
public class UserRepository : RepositoryBase<User, long>, IUserRepository
{
    protected UserRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}