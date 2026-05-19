using Core.Attributes;
using DataAccess.DataContext;
using DataAccess.Entities;
using DataAccess.Repositories.BaseRepository;

namespace DataAccess.Repositories.DepartmentRepositories;
[Injectable]
public class SubDepartmentRepository : RepositoryBase<SubDepartment,long> , ISubDepartmentRepository
{
    public SubDepartmentRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}