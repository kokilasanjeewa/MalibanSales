using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public interface IBranchRepository : IRepositoryBase<BranchLocation>
  {
    IQueryable<BranchLocation> BranchLocation { get; }
    Task<PagedList<BranchLocation>> GetBranchLocationsAsync(UserParams userParams);
  }
}