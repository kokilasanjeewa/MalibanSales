using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public interface IEmployeeRepository : IRepositoryBase<MI_Employer_Employee>
  {
    IQueryable<MI_Employer_Employee> Employees { get; }
    Task<PagedList<MI_Employer_Employee>> GetEmployersAsync(UserParams userParams, int status);
    Task<MI_Employer_Employee> GetEmployeeAsync(string barcode);

  }
}