using System.Threading.Tasks;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public interface IPurchaseRequestDetailRepository : IRepositoryBase<PurchaseRequestDetail>
  {
    PagedList<PurchaseRequestDetail> GetPurchaseRequestDetailsAsync(UserParams userParams, int? purchaseRequestHeaderId);
  }
}