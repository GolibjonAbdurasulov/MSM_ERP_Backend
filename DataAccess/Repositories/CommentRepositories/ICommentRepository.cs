using DataAccess.Entities;
using DataAccess.Repositories.BaseRepository;

namespace DataAccess.Repositories.CommentRepositories;

public interface ICommentRepository : IRepositoryBase<Comment,long>
{
    
}