using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public class EFStoreRepository : RepositoryBase<Store>, IStoreRepository
  {
    public EFStoreRepository(DataContext ctx) : base(ctx)
    {
      context = ctx;
    }
    public IQueryable<Store> Store => context.Stores;
    public async Task<PagedList<Store>> GetStoresAsync(UserParams userParams)
    {
      var products = context.Stores;
      return await PagedList<Store>.CreateAsync(products, userParams.PageNumber, userParams.PageSize);
    }
  }
}