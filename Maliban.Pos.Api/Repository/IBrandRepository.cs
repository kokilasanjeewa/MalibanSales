using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public interface IBrandRepository : IRepositoryBase<Brand>
  {
    IQueryable<Brand> Brand { get; }
    Task<PagedList<Brand>> GetBrandsAsync(UserParams userParams,bool status);
  }
}