using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public interface IStoreRepository : IRepositoryBase<Store>
  {
    IQueryable<Store> Store { get; }
    Task<PagedList<Store>> GetStoresAsync(UserParams userParams);
  }
}