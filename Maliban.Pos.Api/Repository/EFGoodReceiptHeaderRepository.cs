using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Maliban.Pos.Api.Repository
{
  public class EFGoodReceiptHeaderRepository : RepositoryBase<GoodReceptNoteHeader>, IGoodReceiptHeaderRepository
  {
    private readonly ISortHelper<GoodReceptNoteHeader> _sortHelper;

    public EFGoodReceiptHeaderRepository(DataContext ctx, ISortHelper<GoodReceptNoteHeader> sortHelper) : base(ctx)
    {
      context = ctx;
      _sortHelper = sortHelper;
    }
    public async Task<PagedList<GoodReceptNoteHeader>> GetGoodReceptNoteHeadersAsync(UserParams userParams, int? branchLocationId)
    {
      // get header data (on branch location id)
      //  string header = _httpContextAccessor.HttpContext.Request.Headers["BranchLocationId"];
      var purchaseOrderHeaders = context.GoodReceptNoteHeaders.Where(s => s.BranchLocationId == branchLocationId)
      .Include(s => s.Supplier).Include(p => p.PurchaseOrderHeader).Include(a => a.ApplicationUser).OrderByDescending(g => g.GoodReceptNoteHeaderId).AsNoTracking();
      if (purchaseOrderHeaders.Count() > 0)
      {
        _sortHelper.ApplySort(ref purchaseOrderHeaders, userParams.OrderBy);
        //_sortHelper.SearchByName(ref suppliers,x => x.SupplierName.ToLower().Contains(userParams.Filter.Trim().ToLower()));
         SearchByName(ref purchaseOrderHeaders, userParams.Filter);
        //ApplySort(ref suppliers, userParams.OrderBy);
      }
      return await PagedList<GoodReceptNoteHeader>.CreateAsync(purchaseOrderHeaders, userParams.PageNumber, userParams.PageSize);
    }
        private void SearchByName(ref IQueryable<GoodReceptNoteHeader> products, string code)
        {
            if (!products.Any() || string.IsNullOrWhiteSpace(code))
                return;

            //products = products.Where(o => o.Barcode.ToLower().Contains(code.Trim().ToLower()) || o.ProductName.ToLower().Contains(code.Trim().ToLower()));
            products = products.Where(o => o.GoodReceptNoteReferenceNo.ToLower().Contains(code.Trim().ToLower())).Count() > 0 ? products.Where(o => o.GoodReceptNoteReferenceNo.ToLower().Contains(code.Trim().ToLower())) : products.Where(o => o.GoodReceptNoteHeaderId.ToString().ToLower().Contains(code.Trim().ToLower()));

        }
    }
}