using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public class EFStoreAreaRepository : RepositoryBase<StoreArea>, IStoreAreaRepository
  {
    public EFStoreAreaRepository(DataContext ctx) : base(ctx)
    {
      context = ctx;
    }
    public IQueryable<StoreArea> StoreArea => context.StoreAreas;
    public async Task<PagedList<StoreArea>> GetStoreAreasAsync(UserParams userParams)
    {
      var storeareas = context.StoreAreas;
      return await PagedList<StoreArea>.CreateAsync(storeareas, userParams.PageNumber, userParams.PageSize);
    }
  }
}