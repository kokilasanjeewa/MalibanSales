using System;
using System.Collections.Generic;
using System.Linq;
using Maliban.Pos.Api.Dtos;

namespace Maliban.Pos.Api.Models
{
  public class PurchaseOrder
  {
    private List<PurchaseOrderDetail> lineCollection = new List<PurchaseOrderDetail>();
    // private SavePurchaseOrderHeaderDto headerCollection = new SavePurchaseOrderHeaderDto();

    /*   public virtual void AddHeder(SavePurchaseOrderHeaderDto purchaseOrderHeader)
      {
        headerCollection = purchaseOrderHeader;
      } */
    public virtual void AddItem(decimal quantity, SupplierPrice supplierPrice, DateTime ProductExpiryDate, decimal approvequantity = 0, cApprovalType approvalType = 0)
    {
      PurchaseOrderDetail line = lineCollection
      .Where(p => p.Product.ProductId == supplierPrice.ProductId && p.SupplierPriceId== supplierPrice.SupplierPriceId && p.ProductExpiryDate== ProductExpiryDate).FirstOrDefault();

      if (line == null)
      {
                lineCollection.Add(new PurchaseOrderDetail
                {
                    ProductId = supplierPrice.ProductId,
                    Quantity = quantity,
                    ApproveQuantity = approvequantity,
                    cApprovalType = approvalType,
                    Product = supplierPrice.Product,
                    SupplierPrice = supplierPrice,
                    SupplierPriceId = supplierPrice.SupplierPriceId,
                    Amount = approvequantity > 0 ? (approvequantity) * supplierPrice.UnitPrice:(quantity) * supplierPrice.UnitPrice,
                    ProductExpiryDate = ProductExpiryDate,
                });
      }
      if (line != null)
      {
        line.Quantity += quantity;
        line.Amount = line.ApproveQuantity >0? line.ApproveQuantity * supplierPrice.UnitPrice: line.Quantity * supplierPrice.UnitPrice;
      }
    }
    public virtual void RemoveItem(Product product, decimal quantity, SupplierPrice supplierPrice, DateTime ProductExpiryDate)
    {
      PurchaseOrderDetail line = lineCollection
      .Where(p => p.Product.ProductId == product.ProductId && p.SupplierPrice.SupplierPriceId==supplierPrice.SupplierPriceId && p.ProductExpiryDate == ProductExpiryDate).FirstOrDefault();
      if (line != null && line.Quantity > 0)
      {
        line.Quantity -= quantity;
      }
    }
    public virtual void ApproveQuantity(Product product, decimal approveQuantity1, decimal adjustmentQuantity1, SupplierPrice supplierPrice, DateTime ProductExpiryDate)
        {
            PurchaseOrderDetail line = lineCollection
            .Where(p => p.Product.ProductId == supplierPrice.ProductId && p.SupplierPriceId == supplierPrice.SupplierPriceId && p.ProductExpiryDate == ProductExpiryDate).FirstOrDefault();
            decimal approveQuantity = Math.Abs(approveQuantity1);
            decimal adjustmentQuantity = Math.Abs(adjustmentQuantity1);
            if (line != null && approveQuantity <= adjustmentQuantity)
            {
                line.ApproveQuantity = approveQuantity1;
                //line.cApprovalType = cApprovalType.Approve;
            }
        }
    public virtual void ApproveStatus(Product product, int approveStatus, decimal adjustmentQuantity1, SupplierPrice supplierPrice, DateTime ProductExpiryDate)
        {
            PurchaseOrderDetail line = lineCollection
            .Where(p => p.Product.ProductId == supplierPrice.ProductId && p.SupplierPriceId == supplierPrice.SupplierPriceId && p.ProductExpiryDate == ProductExpiryDate).FirstOrDefault();
            decimal approveQuantity = Math.Abs(line.ApproveQuantity);
            decimal adjustmentQuantity = Math.Abs(line.Quantity);
            if (line != null && approveQuantity <= adjustmentQuantity)
            {
                if (approveStatus == 1)
                    line.cApprovalType = cApprovalType.Approve;
                if (approveStatus == 2)
                    line.cApprovalType = cApprovalType.Reject;
            }
        }
    public virtual void RemoveLine(Product product, SupplierPrice supplierPrice, DateTime ProductExpiryDate) =>
    lineCollection.RemoveAll(l => l.Product.ProductId == product.ProductId && l.SupplierPrice.SupplierPriceId==supplierPrice.SupplierPriceId && l.ProductExpiryDate == ProductExpiryDate);
    public virtual void Clear() => lineCollection.Clear();

    public virtual decimal? ComputeDiscountValue => lineCollection.Sum(e => (e.SupplierPrice.UnitPrice * e.SupplierPrice.DiscountPrice) * (e.ApproveQuantity > 0?e.ApproveQuantity:e.Quantity));
    public virtual decimal? ComputeGrandTotalValue => lineCollection.Sum(e => (e.SupplierPrice.UnitPrice - e.SupplierPrice.UnitPrice * e.SupplierPrice.DiscountPrice) * (e.ApproveQuantity > 0 ? e.ApproveQuantity : e.Quantity));
    public virtual decimal TotalQuantity => lineCollection.Sum(e => e.ApproveQuantity > 0 ? e.ApproveQuantity : e.Quantity);
    public virtual decimal? ComputeTotalValue => lineCollection.Sum(e => e.Amount);
    public virtual int Count => lineCollection.Count;
    public virtual ICollection<PurchaseOrderDetail> Lines => lineCollection;
    //  public virtual SavePurchaseOrderHeaderDto SaveHeader => headerCollection;


  }

}
