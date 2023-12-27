using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Maliban.Pos.Api.Repository
{
  public class EFPurchaseReturnHeaderRepository : RepositoryBase<PurchaseReturnHeader>, IPurchaseReturnHeaderRepository
  {
    private readonly ISortHelper<PurchaseReturnHeader> _sortHelper;
    public EFPurchaseReturnHeaderRepository(DataContext ctx, ISortHelper<PurchaseReturnHeader> sortHelper) : base(ctx)
    {
      context = ctx;
      _sortHelper = sortHelper;
    }
    public async Task<PagedList<PurchaseReturnHeader>> GetPurchaseReturnHeadersAsync(UserParams userParams, int? branchLocationId)
    {
      // get header data (on branch location id)
      //  string header = _httpContextAccessor.HttpContext.Request.Headers["BranchLocationId"];
      var purchaseReturnHeaders = context.PurchaseReturnHeaders.Where(s => s.BranchLocationId == branchLocationId)
      .Include(p => p.Lines).Include(r => r.ReturnReason).Include(p => p.Supplier).AsNoTracking();
      if (purchaseReturnHeaders.Count() > 0)
      {
        _sortHelper.ApplySort(ref purchaseReturnHeaders, userParams.OrderBy);
        //_sortHelper.SearchByName(ref suppliers,x => x.SupplierName.ToLower().Contains(userParams.Filter.Trim().ToLower()));
        // SearchByName(ref suppliers, userParams.Filter);
        //ApplySort(ref suppliers, userParams.OrderBy);
      }
      return await PagedList<PurchaseReturnHeader>.CreateAsync(purchaseReturnHeaders, userParams.PageNumber, userParams.PageSize);
    }
  }
}