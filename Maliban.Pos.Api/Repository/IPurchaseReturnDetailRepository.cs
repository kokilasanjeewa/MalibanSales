using System.Threading.Tasks;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public interface IPurchaseReturnDetailRepository : IRepositoryBase<PurchaseReturnDetail>
  {
    PagedList<PurchaseReturnDetail> GetPurchaseReturnDetailsAsync(UserParams userParams, int? purchaseReturnHeaderId);
  }
}