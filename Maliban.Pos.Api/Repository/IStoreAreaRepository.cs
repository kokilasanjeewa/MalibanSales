using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public interface IStoreAreaRepository : IRepositoryBase<StoreArea>
  {
    IQueryable<StoreArea> StoreArea { get; }
    Task<PagedList<StoreArea>> GetStoreAreasAsync(UserParams userParams);
  }
}