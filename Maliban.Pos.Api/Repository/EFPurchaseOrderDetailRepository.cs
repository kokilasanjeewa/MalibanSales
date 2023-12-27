using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Maliban.Pos.Api.Repository
{
  public class EFPurchaseOrderDetailRepository : RepositoryBase<PurchaseOrderDetail>, IPurchaseOrderDetailRepository
  {
    private readonly ISortHelper<PurchaseOrderDetail> _sortHelper;
    public EFPurchaseOrderDetailRepository(DataContext ctx, ISortHelper<PurchaseOrderDetail> sortHelper) : base(ctx)
    {
      context = ctx;
      _sortHelper = sortHelper;
    }
    public PagedList<PurchaseOrderDetail> GetPurchaseOrderDetailsAsync(UserParams userParams, int? purchaseOrderHeaderId)
    {
      // get header data (on branch location id)
      //  string header = _httpContextAccessor.HttpContext.Request.Headers["BranchLocationId"];
      var purchaseOrderDetails = context.PurchaseOrderDetails.Where(s => s.PurchaseOrderHeaderId == purchaseOrderHeaderId).Select(c => new{ c.PurchaseOrderDetailId,c.Active,c.ProductId,c.Product,c.PurchaseOrderHeaderId,c.PurchaseOrderHeaders,c.SupplierPriceId,c.SupplierPrice,c.Amount,c.BonusQuantity,c.CurrentQuantity,c.FreeQuantity,c.GRNQuantity,c.IsDeleted,c.Quantity,c.ReturnQuantity,c.CreateDate});
      IList<PurchaseOrderDetail> r = new List<PurchaseOrderDetail>();
      foreach (var item in purchaseOrderDetails)
      {
        PurchaseOrderDetail p = new PurchaseOrderDetail
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
      }
      return PagedList<PurchaseOrderDetail>.CreateQAsync(r, userParams.PageNumber, userParams.PageSize);
    }
  }
}