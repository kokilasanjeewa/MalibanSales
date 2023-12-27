using System.Linq;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
    public interface IDepartmentRepository: IRepositoryBase<Department>
    {
        IQueryable<Department> Department { get; }

    }
}