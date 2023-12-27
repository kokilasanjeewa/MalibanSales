using System.Threading.Tasks;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public interface IPurchaseOrderHeaderRepository : IRepositoryBase<PurchaseOrderHeader>
  {
    Task<PagedList<PurchaseOrderHeader>> GetPurchaseOrderHeadersAsync(UserParams userParams, int? branchLocationId);
  }
}