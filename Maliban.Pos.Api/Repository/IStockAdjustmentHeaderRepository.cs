using System.Threading.Tasks;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public interface IStockAdjustmentHeaderRepository : IRepositoryBase<StockAdjustmentHeader>
  {
    Task<PagedList<StockAdjustmentHeader>> GetStockAdjustmentHeadersAsync(UserParams userParams, int? branchLocationId);
  }
}