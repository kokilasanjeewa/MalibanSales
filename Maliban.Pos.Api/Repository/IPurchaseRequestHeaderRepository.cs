using System.Threading.Tasks;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public interface IPurchaseRequestHeaderRepository : IRepositoryBase<PurchaseRequestHeader>
  {
    Task<PagedList<PurchaseRequestHeader>> GetPurchaseRequestHeadersAsync(UserParams userParams, int? branchLocationId);
  }
}