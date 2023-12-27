using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Maliban.Pos.Api.Models
{
  public class PurchaseReturn
  {
    private List<PurchaseReturnDetail> lineCollection = new List<PurchaseReturnDetail>();
    // private SavePurchaseOrderHeaderDto headerCollection = new SavePurchaseOrderHeaderDto();

    /*   public virtual void AddHeder(SavePurchaseOrderHeaderDto purchaseOrderHeader)
      {
        headerCollection = purchaseOrderHeader;
      } */
    public virtual void AddItem(decimal quantity, Price price, [Optional] DateTime productExpireDate,Int64 transactionDetailId)
    {
      PurchaseReturnDetail line = lineCollection
      .Where(p => p.Product.ProductId == price.ProductId && p.PriceId==price.PriceId && p.TransactionDetailId== transactionDetailId).FirstOrDefault();

      if (line == null)
      {
        lineCollection.Add(new PurchaseReturnDetail
        {
          ProductId = price.ProductId,
          ReturnQuantity = quantity,
          Product = price.Product,
          ReturnPrice = price.ModifiedUnitPrice,
          PriceId = price.PriceId,
          Price = price,
          ProductExpireDate = productExpireDate,
          SubTotal = quantity * price.ModifiedUnitPrice,
          TransactionDetailId = transactionDetailId,
        });
      }
      else
      {
        line.ReturnQuantity += quantity;
        line.SubTotal = line.ReturnQuantity * price.ModifiedUnitPrice;
      }
    }
    public virtual void RemoveItem(Product product, decimal quantity)
    {
      PurchaseReturnDetail line = lineCollection
      .Where(p => p.Product.ProductId == product.ProductId).FirstOrDefault();
      if (line != null && line.SubTotal > 0)
      {
        line.SubTotal -= quantity;
      }
    }
    public virtual void RemoveLine(Product product,Price price) =>
    lineCollection.RemoveAll(l => l.Product.ProductId == product.ProductId && l.ReturnPrice == price.ModifiedUnitPrice);
    public virtual void Clear() => lineCollection.Clear();
    public virtual decimal? ComputeDiscountValue => lineCollection.Sum(e => (e.Price.ModifiedUnitPrice * e.Price.DiscountPrice) * e.ReturnQuantity);
    public virtual decimal? ComputeGrandTotalValue => lineCollection.Sum(e => (e.Price.ModifiedUnitPrice - e.Price.ModifiedUnitPrice * e.Price.DiscountPrice) * e.ReturnQuantity);
    public virtual decimal TotalQuantity => lineCollection.Sum(e => e.ReturnQuantity);
    public virtual decimal? ComputeTotalValue => lineCollection.Sum(e => e.SubTotal);
    public virtual int Count => lineCollection.Count;
    public virtual ICollection<PurchaseReturnDetail> Lines => lineCollection;
    //  public virtual SavePurchaseOrderHeaderDto SaveHeader => headerCollection;

  }
}