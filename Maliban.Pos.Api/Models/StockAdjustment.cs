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
    public class StockAdjustment
    {
        private List<StockAdjustmentDetail> lineCollection = new List<StockAdjustmentDetail>();
        public virtual void AddItem(Product product, decimal quantity, Price price, decimal stockQty, string status, [Optional] DateTime productExpireDate, Int64 transactionDetailId, decimal approvequantity = 0, cApprovalType approvalType = 0)
        {
            StockAdjustmentDetail line = lineCollection
            .Where(p => p.Product.ProductId == product.ProductId && p.CostPrice == price.ModifiedUnitPrice && p.TransactionDetailId == transactionDetailId).FirstOrDefault();
            if (line == null)
            {
                lineCollection.Add(new StockAdjustmentDetail
                {
                    Product = product,
                    PriceId = price.PriceId,
                    Price = price,
                    ProductId = product.ProductId,
                    CostPrice = price.ModifiedUnitPrice,
                    RetailPrice = price.MaximumRetailPrice,
                    Barcode = product.Barcode,
                    ProductName = product.ProductName,
                    ApproveQuantity = approvequantity,
                    cApprovalType = approvalType,
                    AdjustmentQuantity = quantity,
                    PreviousQuantity = stockQty,
                    CurrentQuantity = stockQty - quantity,
                    StockAdjustmentTotal = (price.ModifiedUnitPrice) * quantity,
                    ProductExpireDate = productExpireDate,
                    AdjustmentType = status,
                    TransactionDetailId = transactionDetailId,
                });
            }
            else
            {
                line.AdjustmentQuantity += quantity;
                line.PreviousQuantity -= quantity;
                line.StockAdjustmentTotal = (line.CostPrice) * line.AdjustmentQuantity;

            }
        }
        public virtual void UpdateQuantity(Product product, decimal quantity, decimal stockQty, Price price)
        {
            StockAdjustmentDetail line = lineCollection
            .Where(p => p.Product.ProductId == product.ProductId && p.CostPrice == price.ModifiedUnitPrice).FirstOrDefault();

            if (line != null && line.AdjustmentQuantity > 0)
            {
                line.AdjustmentQuantity = quantity;
                line.StockAdjustmentTotal = (line.CostPrice) * quantity;
            }
        }
        public virtual void ApproveStatus(Product product, int approveStatus, decimal adjustmentQuantity1, Price price, Int64 transactionDetailId)
        {
            StockAdjustmentDetail line = lineCollection
            .Where(p => p.Product.ProductId == product.ProductId && p.PriceId == price.PriceId && p.TransactionDetailId == transactionDetailId).FirstOrDefault();
            decimal approveQuantity = Math.Abs(line.AdjustmentQuantity);
            decimal adjustmentQuantity = Math.Abs(line.AdjustmentQuantity);
            if (line != null && approveQuantity <= adjustmentQuantity)
            {
                if (approveStatus == 1)
                    line.cApprovalType = cApprovalType.Approve;
                if (approveStatus == 2)
                    line.cApprovalType = cApprovalType.Reject;
            }
        }
        public virtual void RemoveItem(Product product, decimal quantity)
        {
            StockAdjustmentDetail line = lineCollection
            .Where(p => p.Product.ProductId == product.ProductId).FirstOrDefault();
            if (line != null && line.AdjustmentQuantity > 0)
            {
                line.AdjustmentQuantity = line.AdjustmentQuantity - quantity;
                line.StockAdjustmentTotal = (line.CostPrice) * line.AdjustmentQuantity;
            }

        }
        public virtual void RemoveLine(Product product, Price price) =>
        lineCollection.RemoveAll(l => l.Product.ProductId == product.ProductId && l.CostPrice == price.ModifiedUnitPrice);
        public virtual decimal? ComputeDiscountValue => lineCollection.Sum(e => (e.CostPrice) * e.AdjustmentQuantity);
        public virtual decimal ComputeGrandTotalValue => lineCollection.Sum(e => (e.CostPrice) * e.AdjustmentQuantity);
        public virtual decimal TotalQuantity => lineCollection.Sum(e => e.AdjustmentQuantity);
        public virtual void Clear() => lineCollection.Clear();
        public virtual ICollection<StockAdjustmentDetail> Lines => lineCollection;
        public virtual decimal? ComputeTotal => lineCollection.Sum(e => e.CostPrice * e.AdjustmentQuantity);
        public virtual int Count => lineCollection.Count;
    }




}
