using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Maliban.Pos.Api.Repository
{
  public class EFBinRepository : RepositoryBase<Bin>, IBinRepository
  {
    public EFBinRepository(DataContext ctx) : base(ctx)
    {
      context = ctx;
    }
    public IQueryable<Bin> Bin => context.Bins;
    public async Task<PagedList<Bin>> GetBinsAsync(UserParams userParams)
    {
      var bins = context.Bins;
      return await PagedList<Bin>.CreateAsync(bins, userParams.PageNumber, userParams.PageSize);
    }
  }
}