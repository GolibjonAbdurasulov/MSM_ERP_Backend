using DataAccess.Entities;
using DataAccess.Repositories.BaseRepository;

namespace DataAccess.Repositories.UserRepositories;

public interface IUserRepository : IRepositoryBase<User, long>
{
    
}