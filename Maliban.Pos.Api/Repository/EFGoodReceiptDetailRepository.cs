using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Maliban.Pos.Api.Repository
{
  public class EFGoodReceiptDetailRepository : RepositoryBase<GoodReceptNoteDetail>, IGoodReceiptDetailRepository
  {
    private readonly ISortHelper<GoodReceptNoteDetail> _sortHelper;
    public EFGoodReceiptDetailRepository(DataContext ctx, ISortHelper<GoodReceptNoteDetail> sortHelper) : base(ctx)
    {
      context = ctx;
      _sortHelper = sortHelper;
    }
    public PagedList<GoodReceptNoteDetail> GetGoodReceiptDetailsAsync(UserParams userParams, int? goodReceptNoteHeaderId)
    {
      // get header data (on branch location id)
      //  string header = _httpContextAccessor.HttpContext.Request.Headers["BranchLocationId"];
      var goodreceptnoteDetails = context.GoodReceptNoteDetails.Where(s => s.GoodReceptNoteHeaderId == goodReceptNoteHeaderId)
      .Select(g => new { g.GoodReceptNoteDetailId, g.Active, g.BalanceItemQuantity, g.BatchNumber, g.CostPrice, g.CreateDate, g.FreeItemQuantity, g.GoodReceptNoteHeader, g.GoodReceptNoteHeaderId, g.PurchaseOrderHeader, g.PurchaseOrderHeaderId, g.Product, g.ProductId, g.SupplierPriceId, g.SupplierPrice, g.IsDeleted, g.CreatedBy, g.ProductExpiryDate, g.GroupDiscountAmount, g.InvoiceItemDate, g.ItemQuantity, g.RetailPrice });
      IList<GoodReceptNoteDetail> r = new List<GoodReceptNoteDetail>();
      foreach (var item in goodreceptnoteDetails)
      {
        GoodReceptNoteDetail p = new GoodReceptNoteDetail
        {
          GoodReceptNoteDetailId =item.GoodReceptNoteDetailId,
          Active =item.Active,
          BalanceItemQuantity =item.BalanceItemQuantity,
          BatchNumber =item.BatchNumber,
          CostPrice =item.CostPrice,
          CreateDate =item.CreateDate,
          FreeItemQuantity =item.FreeItemQuantity,
          GoodReceptNoteHeader =item.GoodReceptNoteHeader,
          GoodReceptNoteHeaderId =item.GoodReceptNoteHeaderId,
          PurchaseOrderHeader =item.PurchaseOrderHeader,
          PurchaseOrderHeaderId =item.PurchaseOrderHeaderId,
          Product =item.Product,
          ProductId =item.ProductId,
          SupplierPriceId =item.SupplierPriceId,
          SupplierPrice =item.SupplierPrice,
          IsDeleted =item.IsDeleted,
          CreatedBy =item.CreatedBy,
          ProductExpiryDate =item.ProductExpiryDate,
          GroupDiscountAmount =item.GroupDiscountAmount,
          InvoiceItemDate =item.InvoiceItemDate,
          ItemQuantity =item.ItemQuantity,
          RetailPrice =item.RetailPrice
        };
        r.Add(p);
      }
      if (goodreceptnoteDetails.Count() > 0)
      {
        // _sortHelper.ApplySort(ref r, userParams.OrderBy);
        //_sortHelper.SearchByName(ref suppliers,x => x.SupplierName.ToLower().Contains(userParams.Filter.Trim().ToLower()));
        // SearchByName(ref suppliers, userParams.Filter);
        //ApplySort(ref suppliers, userParams.OrderBy);
      }
      return PagedList<GoodReceptNoteDetail>.CreateQAsync(r, userParams.PageNumber, userParams.PageSize);
    }
  }
}