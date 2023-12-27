using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public class EFRackRepository : RepositoryBase<Rack>,IRackRepository
  {
    public EFRackRepository(DataContext ctx) : base(ctx)
    {
      context = ctx;
    }
    public IQueryable<Rack> Rack => context.Racks;
    public async Task<PagedList<Rack>> GetRacksAsync(UserParams userParams)
    {
      var racks = context.Racks;
      return await PagedList<Rack>.CreateAsync(racks, userParams.PageNumber, userParams.PageSize);
    }
  }
}