using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Maliban.Pos.Api.Repository
{
  public class EFStockAdjustmentHeaderRepository : RepositoryBase<StockAdjustmentHeader>, IStockAdjustmentHeaderRepository
  {

    public EFStockAdjustmentHeaderRepository(DataContext ctx) : base(ctx)
    {
      context = ctx;
    }
    public async Task<PagedList<StockAdjustmentHeader>> GetStockAdjustmentHeadersAsync(UserParams userParams, int? branchLocationId)
    {
      // get header data (on branch location id)
      //  string header = _httpContextAccessor.HttpContext.Request.Headers["BranchLocationId"];
      var purchaseOrderHeaders = context.StockAdjustmentHeaders.Where(s => s.BranchLocationId == branchLocationId)
      .Include(s => s.AdjustmentType).OrderByDescending(t => t.StockAdjustmentHeaderId).AsNoTracking();
      if (purchaseOrderHeaders.Count() > 0)
      {
        //_sortHelper.ApplySort(ref purchaseOrderHeaders, userParams.OrderBy);
        //_sortHelper.SearchByName(ref suppliers,x => x.SupplierName.ToLower().Contains(userParams.Filter.Trim().ToLower()));
        // SearchByName(ref suppliers, userParams.Filter);
        //ApplySort(ref suppliers, userParams.OrderBy);
      }
      return await PagedList<StockAdjustmentHeader>.CreateAsync(purchaseOrderHeaders, userParams.PageNumber, userParams.PageSize);
    }
  }
}