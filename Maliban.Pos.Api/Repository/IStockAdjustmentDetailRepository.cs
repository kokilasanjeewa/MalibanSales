using System.Threading.Tasks;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public interface IStockAdjustmentDetailRepository : IRepositoryBase<StockAdjustmentDetail>
  {
    Task<PagedList<StockAdjustmentDetail>> GetStockAdjustmentDetailsAsync(UserParams userParams, int? stockadjustmentHeaderId);
  }
}