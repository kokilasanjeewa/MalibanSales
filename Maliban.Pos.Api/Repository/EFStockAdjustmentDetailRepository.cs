using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Maliban.Pos.Api.Repository
{
  public class EFStockAdjustmentDetailRepository : RepositoryBase<StockAdjustmentDetail>, IStockAdjustmentDetailRepository
  {
    public EFStockAdjustmentDetailRepository(DataContext ctx) : base(ctx)
    {
      context = ctx;
    }
    public async Task<PagedList<StockAdjustmentDetail>> GetStockAdjustmentDetailsAsync(UserParams userParams, int? stockadjustmentHeaderId)
    {
      // get header data (on branch location id)
      //  string header = _httpContextAccessor.HttpContext.Request.Headers["BranchLocationId"];
      var purchaseOrderDetails = await context.StockAdjustmentDetails.Where(s => s.StockAdjustmentHeaderId == stockadjustmentHeaderId).Include(p => p.StockAdjustmentHeader).OrderByDescending(t => t.StockAdjustmentDetailId).ToListAsync();
      /*    IList<StockAdjustmentDetail> r = new List<StockAdjustmentDetail>();
         foreach (var item in purchaseOrderDetails)
         {
           StockAdjustmentDetail p = new StockAdjustmentDetail
           {
             Product = item.Product,
             ProductId = item.ProductId,
             Active = item.Active,
             PurchaseOrderHeaderId = item.PurchaseOrderHeaderId,
             PurchaseOrderHeaders = item.PurchaseOrderHeaders,
             SupplierPriceId=item.SupplierPriceId,
             SupplierPrice= item.SupplierPrice,
             PurchaseOrderDetailId =item.PurchaseOrderDetailId,
             Amount = item.Amount,
             BonusQuantity = item.BonusQuantity,
             CurrentQuantity = item.CurrentQuantity,
             FreeQuantity = item.FreeQuantity,
             GRNQuantity = item.GRNQuantity,
             IsDeleted = item.IsDeleted,
             Quantity = item.Quantity,
             ReturnQuantity = item.ReturnQuantity,
             CreateDate = item.CreateDate
           };
           r.Add(p);
         }
         if (purchaseOrderDetails.Count() > 0)
         {
          // _sortHelper.ApplySort(ref r, userParams.OrderBy);
           //_sortHelper.SearchByName(ref suppliers,x => x.SupplierName.ToLower().Contains(userParams.Filter.Trim().ToLower()));
           // SearchByName(ref suppliers, userParams.Filter);
           //ApplySort(ref suppliers, userParams.OrderBy);
         } */
      return PagedList<StockAdjustmentDetail>.CreateQAsync(purchaseOrderDetails, userParams.PageNumber, userParams.PageSize);
    }
  }
}