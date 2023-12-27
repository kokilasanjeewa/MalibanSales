using System.Threading.Tasks;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public interface IPurchaseOrderDetailRepository : IRepositoryBase<PurchaseOrderDetail>
  {
    PagedList<PurchaseOrderDetail> GetPurchaseOrderDetailsAsync(UserParams userParams, int? purchaseOrderHeaderId);
  }
}