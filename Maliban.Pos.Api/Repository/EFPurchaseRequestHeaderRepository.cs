using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Maliban.Pos.Api.Repository
{
  public class EFPurchaseRequestHeaderRepository : RepositoryBase<PurchaseRequestHeader>, IPurchaseRequestHeaderRepository
  {
    private readonly ISortHelper<PurchaseRequestHeader> _sortHelper;
    public EFPurchaseRequestHeaderRepository(DataContext ctx, ISortHelper<PurchaseRequestHeader> sortHelper) : base(ctx)
    {
      context = ctx;
      _sortHelper = sortHelper;
    }
    public async Task<PagedList<PurchaseRequestHeader>> GetPurchaseRequestHeadersAsync(UserParams userParams, int? branchLocationId)
    {
      // get header data (on branch location id)
      //  string header = _httpContextAccessor.HttpContext.Request.Headers["BranchLocationId"];
      var purchaseRequestHeaders =  context.PurchaseRequestHeaders.Where(s => s.BranchLocationId == branchLocationId)
      .Include(p => p.Department).Include(p => p.Section).AsNoTracking();
        if (purchaseRequestHeaders.Count() > 0)
      {
        _sortHelper.ApplySort(ref purchaseRequestHeaders, userParams.OrderBy);
        //_sortHelper.SearchByName(ref suppliers,x => x.SupplierName.ToLower().Contains(userParams.Filter.Trim().ToLower()));
        // SearchByName(ref suppliers, userParams.Filter);
        //ApplySort(ref suppliers, userParams.OrderBy);
      }
      return await PagedList<PurchaseRequestHeader>.CreateAsync(purchaseRequestHeaders, userParams.PageNumber, userParams.PageSize);
    }
  }
}