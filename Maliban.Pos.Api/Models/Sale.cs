using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Maliban.Pos.Api.DataAccess;
using System.Runtime.InteropServices;

namespace Maliban.Pos.Api.Models
{
  public class Sale
  {
    private List<SaleDetail> lineCollection = new List<SaleDetail>();
    public virtual void AddItem(Product product, decimal quantity, Price price, decimal stockQty, [Optional] bool activeBill,[Optional] decimal returnQuantity,[Optional] DateTime productExpireDate, Int64 transactionDetailId)
    {
      SaleDetail line = lineCollection
      .Where(p => p.Product.ProductId == product.ProductId && p.ItemPrice == price.SalePrice && p.CostPrice == price.ModifiedUnitPrice && p.TransactionDetailId== transactionDetailId).FirstOrDefault();

      if (line == null && activeBill)
      {
        lineCollection.Add(new SaleDetail
        {
          Product = product,
          PriceId = price.PriceId,
          ProductId = product.ProductId,
          ItemPrice = price.SalePrice,
          CostPrice = price.ModifiedUnitPrice,
            // ItemDiscount = price.DiscountPrecentage,
          ItemDiscount = 0,
          Quantity = quantity,
          ReturnQuantity=returnQuantity,
          StockQuantity = stockQty,
          Total = (price.SalePrice) * (quantity-returnQuantity),
          SubTotal = (price.SalePrice) * (quantity-returnQuantity),
          BillDate = DateTime.Now.Date,
          ProductExpireDate =productExpireDate,
          ActiveBill = activeBill,
          TransactionDetailId= transactionDetailId,
        });
      }
      if (line == null && !activeBill)
      {
        lineCollection.Add(new SaleDetail
        {
          Product = product,
          PriceId = price.PriceId,
          ProductId = product.ProductId,
          ItemPrice = price.SalePrice,
          CostPrice = price.ModifiedUnitPrice,
          // ItemDiscount = price.DiscountPrecentage,
          ItemDiscount = 0,
          Quantity = quantity,
          StockQuantity = stockQty - quantity,
          Total = (price.SalePrice) * quantity,
          SubTotal = (price.SalePrice) * quantity,
          BillDate = DateTime.Now.Date,
          ProductExpireDate =productExpireDate,
          ActiveBill = activeBill,
          TransactionDetailId = transactionDetailId,

        });
      }
      if (line != null)
      {
        line.Quantity += quantity;
        line.StockQuantity -= quantity;
        line.Total = (line.ItemPrice - line.ItemPrice * line.ItemDiscount) * line.Quantity;
        line.SubTotal = (line.ItemPrice) * line.Quantity;

      }
    }
    public virtual void UpdateQuantity(Product product, decimal quantity, decimal stockQty, Price price, Int64 transactionDetailId)
    {
      SaleDetail line = lineCollection
      .Where(p => p.Product.ProductId == product.ProductId && p.ItemPrice == price.SalePrice && p.CostPrice==price.ModifiedUnitPrice && p.TransactionDetailId == transactionDetailId).FirstOrDefault();

      if (line != null && line.Quantity > 0 && !line.ActiveBill)
      {
        line.Quantity = quantity;
        line.SubTotal = (line.ItemPrice) * quantity;
        line.Total = (line.ItemPrice - line.ItemPrice * line.ItemDiscount) * quantity;
        line.StockQuantity = stockQty > quantity ? stockQty - quantity: stockQty;
      }
      if (line != null && quantity <= line.Quantity && line.ActiveBill)
      {
       // line.Quantity = quantity;
        line.ReturnQuantity=quantity;
        line.SubTotal = (line.ItemPrice) * (line.Quantity-quantity);
        line.Total = (line.ItemPrice - line.ItemPrice * line.ItemDiscount) * (line.Quantity-quantity);
        line.StockQuantity = stockQty+quantity;
      }
    }
    public virtual void RemoveItem(Product product, int quantity)
    {
      SaleDetail line = lineCollection
      .Where(p => p.Product.ProductId == product.ProductId).FirstOrDefault();
      if (line != null && line.Quantity > 0)
      {
        line.Quantity = line.Quantity - quantity;
        line.SubTotal = (line.ItemPrice) * line.Quantity;
        line.Total = (line.ItemPrice - line.ItemPrice * line.ItemDiscount) * line.Quantity;
      }

    }
    public virtual void RemoveLine(Product product) =>
    lineCollection.RemoveAll(l => l.Product.ProductId == product.ProductId);
    public virtual decimal? ComputeDiscountValue => lineCollection.Sum(e => (e.ItemPrice * e.ItemDiscount) * (e.Quantity-e.ReturnQuantity));
    public virtual decimal ComputeGrandTotalValue => lineCollection.Sum(e => (e.ItemPrice - e.ItemPrice * e.ItemDiscount) * (e.Quantity-e.ReturnQuantity==0 ? e.ReturnQuantity:e.Quantity-e.ReturnQuantity));
    public virtual decimal TotalQuantity => lineCollection.Sum(e => e.Quantity-e.ReturnQuantity);
    public virtual void Clear() => lineCollection.Clear();
    public virtual ICollection<SaleDetail> Lines => lineCollection;
    public virtual bool BillActive => lineCollection.Any(c => c.ActiveBill);
    public virtual decimal? ComputeTotal => lineCollection.Sum(e => e.ItemPrice * (e.Quantity-e.ReturnQuantity));
    public virtual decimal? ComputeReturnTotal => lineCollection.Sum(e => e.ItemPrice * (e.ReturnQuantity));
    public virtual int Count => lineCollection.Count;
    //public SaleItemDetails Carts { get; set; }
  }




}
