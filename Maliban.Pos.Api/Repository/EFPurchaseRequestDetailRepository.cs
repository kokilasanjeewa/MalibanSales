using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Maliban.Pos.Api.Repository
{
  public class EFPurchaseRequestDetailRepository : RepositoryBase<PurchaseRequestDetail>, IPurchaseRequestDetailRepository
  {
    private readonly ISortHelper<PurchaseRequestDetail> _sortHelper;
    public EFPurchaseRequestDetailRepository(DataContext ctx, ISortHelper<PurchaseRequestDetail> sortHelper) : base(ctx)
    {
      context = ctx;
      _sortHelper = sortHelper;
    }
    public  PagedList<PurchaseRequestDetail> GetPurchaseRequestDetailsAsync(UserParams userParams, int? purchaseRequestHeaderId)
    {
      // get header data (on branch location id)
      //  string header = _httpContextAccessor.HttpContext.Request.Headers["BranchLocationId"];
      var purchaseRequestDetails = context.PurchaseRequestDetails.Where(s => s.PurchaseRequestHeaderId == purchaseRequestHeaderId).Select(c => new {c.Product,c.ProductId,c.Active,c.ApprovedQuantity,c.IsComfirm,c.Quantity,c.RejectQuantity,c.StockQuantity,c.PurchaseRequestDetailId,c.PurchaseRequestDetailUid,c.PurchaseRequestHeader});
      IList<PurchaseRequestDetail> r = new List<PurchaseRequestDetail>();
      foreach(var item in purchaseRequestDetails) {
        PurchaseRequestDetail p = new PurchaseRequestDetail{
              Product=item.Product,
              ProductId=item.ProductId,
              Active=item.Active,
              ApprovedQuantity=item.ApprovedQuantity,
              IsComfirm=item.IsComfirm,
              Quantity=item.Quantity,
              RejectQuantity=item.RejectQuantity,
              StockQuantity=item.StockQuantity,
              PurchaseRequestDetailId=item.PurchaseRequestDetailId,
              PurchaseRequestDetailUid=item.PurchaseRequestDetailUid,
              PurchaseRequestHeader = item.PurchaseRequestHeader
        };
        r.Add(p);
      }
      if (purchaseRequestDetails.Count() > 0)
      {
       // _sortHelper.ApplySort(ref r, userParams.OrderBy);
        //_sortHelper.SearchByName(ref suppliers,x => x.SupplierName.ToLower().Contains(userParams.Filter.Trim().ToLower()));
        // SearchByName(ref suppliers, userParams.Filter);
        //ApplySort(ref suppliers, userParams.OrderBy);
      }
      return  PagedList<PurchaseRequestDetail>.CreateQAsync(r, userParams.PageNumber, userParams.PageSize);
    }
  }
}