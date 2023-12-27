using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Maliban.Pos.Api.Repository
{
  public class EFPurchaseOrderHeaderRepository : RepositoryBase<PurchaseOrderHeader>, IPurchaseOrderHeaderRepository
  {
    private readonly ISortHelper<PurchaseOrderHeader> _sortHelper;

    public EFPurchaseOrderHeaderRepository(DataContext ctx, ISortHelper<PurchaseOrderHeader> sortHelper) : base(ctx)
    {
      context = ctx;
      _sortHelper = sortHelper;
    }
    public async Task<PagedList<PurchaseOrderHeader>> GetPurchaseOrderHeadersAsync(UserParams userParams, int? branchLocationId)
    {
      // get header data (on branch location id)
      //  string header = _httpContextAccessor.HttpContext.Request.Headers["BranchLocationId"];
      var purchaseOrderHeaders = context.PurchaseOrderHeaders.Where(s => s.BranchLocationId == branchLocationId)
      .Include(s => s.Supplier).Include(p => p.PaymentMethod).OrderByDescending(c => c.PurchaseOrderHeaderId).AsNoTracking();
      if (purchaseOrderHeaders.Count() > 0)
      {
        _sortHelper.ApplySort(ref purchaseOrderHeaders, userParams.OrderBy);
        //_sortHelper.SearchByName(ref suppliers,x => x.SupplierName.ToLower().Contains(userParams.Filter.Trim().ToLower()));
         SearchByName(ref purchaseOrderHeaders, userParams.Filter);
        //ApplySort(ref suppliers, userParams.OrderBy);
      }
      return await PagedList<PurchaseOrderHeader>.CreateAsync(purchaseOrderHeaders, userParams.PageNumber, userParams.PageSize);
    }
        private void SearchByName(ref IQueryable<PurchaseOrderHeader> products, string code)
        {
            if (!products.Any() || string.IsNullOrWhiteSpace(code))
                return;

            //products = products.Where(o => o.Barcode.ToLower().Contains(code.Trim().ToLower()) || o.ProductName.ToLower().Contains(code.Trim().ToLower()));
            products = products.Where(o => o.PurchaseOrderReferenceNo.ToLower().Contains(code.Trim().ToLower())).Count() > 0 ? products.Where(o => o.PurchaseOrderReferenceNo.ToLower().Contains(code.Trim().ToLower())) : products.Where(o => o.PurchaseOrderHeaderId.ToString().ToLower().Contains(code.Trim().ToLower()));

        }
    }
}