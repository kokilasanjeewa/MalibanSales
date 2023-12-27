using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Maliban.Pos.Api.Repository
{
  public class EFPurchaseReturnDetailRepository : RepositoryBase<PurchaseReturnDetail>, IPurchaseReturnDetailRepository
  {
    private readonly ISortHelper<PurchaseReturnDetail> _sortHelper;
    public EFPurchaseReturnDetailRepository(DataContext ctx, ISortHelper<PurchaseReturnDetail> sortHelper) : base(ctx)
    {
      context = ctx;
      _sortHelper = sortHelper;
    }
    public  PagedList<PurchaseReturnDetail> GetPurchaseReturnDetailsAsync(UserParams userParams, int? purchaseReturnHeaderId)
    {
      // get header data (on branch location id)
      //  string header = _httpContextAccessor.HttpContext.Request.Headers["BranchLocationId"];
      var purchaseReturnDetails = context.PurchaseReturnDetails.Where(s => s.PurchaseReturnHeaderId == purchaseReturnHeaderId).Select(c => new {c.Product,c.ProductId,c.Active,c.ReturnQuantity,c.PurchaseReturnDetailId,c.PurchaseReturnDetailUid,c.PurchaseReturnHeader});
      IList<PurchaseReturnDetail> r = new List<PurchaseReturnDetail>();
      foreach(var item in purchaseReturnDetails) {
        PurchaseReturnDetail p = new PurchaseReturnDetail{
              Product=item.Product,
              ProductId=item.ProductId,
              Active=item.Active,
              ReturnQuantity=item.ReturnQuantity,
              PurchaseReturnDetailId=item.PurchaseReturnDetailId,
              PurchaseReturnDetailUid=item.PurchaseReturnDetailUid,
              PurchaseReturnHeader = item.PurchaseReturnHeader
        };
        r.Add(p);
      }
      if (purchaseReturnDetails.Count() > 0)
      {
       // _sortHelper.ApplySort(ref r, userParams.OrderBy);
        //_sortHelper.SearchByName(ref suppliers,x => x.SupplierName.ToLower().Contains(userParams.Filter.Trim().ToLower()));
        // SearchByName(ref suppliers, userParams.Filter);
        //ApplySort(ref suppliers, userParams.OrderBy);
      }
      return  PagedList<PurchaseReturnDetail>.CreateQAsync(r, userParams.PageNumber, userParams.PageSize);
    }
  }
}