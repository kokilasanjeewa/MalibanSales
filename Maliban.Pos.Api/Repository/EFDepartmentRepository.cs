using System.Linq;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
     public class EFDepartmentRepository : RepositoryBase<Department>, IDepartmentRepository
  {
    public EFDepartmentRepository(DataContext ctx) : base(ctx)
    {
      context = ctx;
    }
   public IQueryable<Department> Department => context.Departments;

  }
}