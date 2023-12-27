using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  internal class EFBranchRepository : RepositoryBase<BranchLocation>,IBranchRepository
  {

    public EFBranchRepository(DataContext ctx):base(ctx)
    {
      context = ctx;
    }
    public IQueryable<BranchLocation> BranchLocation => context.BranchLocations;

    public async Task<PagedList<BranchLocation>> GetBranchLocationsAsync(UserParams userParams)
    {
      var branchlocations = context.BranchLocations;
      return await PagedList<BranchLocation>.CreateAsync(branchlocations, userParams.PageNumber, userParams.PageSize);
    }
  }
}