using Core.Attributes;
using DataAccess.DataContext;
using DataAccess.Entities;
using DataAccess.Repositories.BaseRepository;

namespace DataAccess.Repositories.CommentRepositories;
[Injectable]
public class CommentRepository : RepositoryBase<Comment,long>, ICommentRepository
{
    public CommentRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}