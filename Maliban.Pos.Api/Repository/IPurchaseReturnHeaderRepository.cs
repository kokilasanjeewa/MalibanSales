using System.Threading.Tasks;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public interface IPurchaseReturnHeaderRepository : IRepositoryBase<PurchaseReturnHeader>
  {
    Task<PagedList<PurchaseReturnHeader>> GetPurchaseReturnHeadersAsync(UserParams userParams, int? branchLocationId);
  }
}