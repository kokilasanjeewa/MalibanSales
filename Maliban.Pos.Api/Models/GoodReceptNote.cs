using System;
using System.Collections.Generic;
using System.Linq;
using Maliban.Pos.Api.DataAccess;

namespace Maliban.Pos.Api.Models
{
    public class GoodReceptNote
    {
        private List<GoodReceptNoteDetail> lineCollection = new List<GoodReceptNoteDetail>();
        public virtual void AddItem(GoodReceptNoteDetail goodReceptNoteDetail)
        {
            GoodReceptNoteDetail line = lineCollection
            .Where(p => p.Product.ProductId == goodReceptNoteDetail.ProductId && p.SupplierPriceId == goodReceptNoteDetail.SupplierPriceId && p.ProductExpiryDate== goodReceptNoteDetail.ProductExpiryDate).FirstOrDefault();
            if (line == null)
            {
                lineCollection.Add(new GoodReceptNoteDetail
                {
                    ProductId = goodReceptNoteDetail.ProductId,
                    Product = goodReceptNoteDetail.Product,
                    SupplierPriceId = goodReceptNoteDetail.SupplierPriceId,
                    SupplierPrice = goodReceptNoteDetail.SupplierPrice,
                    CostPrice = goodReceptNoteDetail.CostPrice,
                    RetailPrice = goodReceptNoteDetail.RetailPrice,
                    ItemQuantity = goodReceptNoteDetail.ItemQuantity,
                    dNetQuantity = goodReceptNoteDetail.dNetQuantity,
                    BalanceItemQuantity = goodReceptNoteDetail.BalanceItemQuantity,
                    FreeItemQuantity = goodReceptNoteDetail.FreeItemQuantity,
                    BatchNumber = goodReceptNoteDetail.BatchNumber,
                    ProductExpiryDate = goodReceptNoteDetail.ProductExpiryDate,
                    GroupDiscountAmount = goodReceptNoteDetail.GroupDiscountAmount,
                    InvoiceItemDate = goodReceptNoteDetail.InvoiceItemDate,
                    CreateDate = goodReceptNoteDetail.CreateDate,
                    GoodReceptNoteLine = goodReceptNoteDetail.GoodReceptNoteLine,
                    CreatedBy = goodReceptNoteDetail.CreatedBy,
                    PurchaseOrderHeaderId = goodReceptNoteDetail.PurchaseOrderHeaderId,
                    PricePrecentage = goodReceptNoteDetail.PricePrecentage   //please uncomment if it's running on production
                                                                             //PricePrecentage = 0   //please comment if it's running on production
                });
            }
            if (line != null)
            {

                line.ItemQuantity += goodReceptNoteDetail.ItemQuantity;
                line.BalanceItemQuantity += goodReceptNoteDetail.BalanceItemQuantity;
                line.FreeItemQuantity += goodReceptNoteDetail.FreeItemQuantity;
                line.GroupDiscountAmount += goodReceptNoteDetail.GroupDiscountAmount;
            }
        }
        public virtual void UpdateQuantity(GoodReceptNoteDetail goodReceptNoteDetail, int quantity)
        {
            GoodReceptNoteDetail line = lineCollection
            .Where(p => p.Product.ProductId == goodReceptNoteDetail.ProductId && p.SupplierPriceId == goodReceptNoteDetail.SupplierPriceId && p.ProductExpiryDate == goodReceptNoteDetail.ProductExpiryDate).FirstOrDefault();
            if (line != null && line.ItemQuantity > 0)
            {
                line.ItemQuantity = quantity;
                line.SupplierPrice.UnitPrice = (line.CostPrice * line.ItemQuantity) /*- (line.GroupDiscountAmount * line.CostPrice * line.ItemQuantity) / 100) */ /(line.ItemQuantity);// remove + line.FreeItemQuantity
            }
        }
        public virtual void RemoveItem(GoodReceptNoteDetail goodReceptNoteDetail)
        {
            GoodReceptNoteDetail line = lineCollection
            .Where(p => p.Product.ProductId == goodReceptNoteDetail.ProductId && p.SupplierPriceId == goodReceptNoteDetail.SupplierPriceId && p.ProductExpiryDate == goodReceptNoteDetail.ProductExpiryDate).FirstOrDefault();

            if (line != null && line.ItemQuantity > 0)
            {
                line.ItemQuantity = line.ItemQuantity - goodReceptNoteDetail.ItemQuantity;
            }

        }
        public virtual void UpdateItem(GoodReceptNoteDetail goodReceptNoteDetail)
        {
            GoodReceptNoteDetail line = lineCollection
            .Where(p => p.Product.ProductId == goodReceptNoteDetail.ProductId && p.SupplierPriceId == goodReceptNoteDetail.SupplierPriceId && p.ProductExpiryDate == goodReceptNoteDetail.ProductExpiryDate).FirstOrDefault();

            if (line != null && line.ItemQuantity > 0)
            {
                line.ProductExpiryDate = goodReceptNoteDetail.ProductExpiryDate;
            }
            if (line != null && line.ItemQuantity == 0 && goodReceptNoteDetail.FreeItemQuantity > 0)
            {
                line.ProductExpiryDate = goodReceptNoteDetail.ProductExpiryDate;

            }
        }
        public virtual void UpdateDiscount(GoodReceptNoteDetail goodReceptNoteDetail)
        {
            GoodReceptNoteDetail line = lineCollection
            .Where(p => p.Product.ProductId == goodReceptNoteDetail.ProductId && p.SupplierPriceId == goodReceptNoteDetail.SupplierPriceId && p.ProductExpiryDate == goodReceptNoteDetail.ProductExpiryDate).FirstOrDefault();

            if (line != null && line.ItemQuantity > 0)
            {
                line.PricePrecentage = goodReceptNoteDetail.PricePrecentage;
            }

        }
        public virtual void UpdateVat(GoodReceptNoteDetail goodReceptNoteDetail,decimal unitPrice)
        {
            GoodReceptNoteDetail line = lineCollection
            .Where(p => p.Product.ProductId == goodReceptNoteDetail.ProductId && p.SupplierPriceId == goodReceptNoteDetail.SupplierPriceId && p.ProductExpiryDate == goodReceptNoteDetail.ProductExpiryDate).FirstOrDefault();
            

            if (line != null && line.ItemQuantity > 0)
                {
                    line.CostPrice = unitPrice * (1 + goodReceptNoteDetail.VATPrecentage / 100);
                    line.VATPrecentage = goodReceptNoteDetail.VATPrecentage;
                    line.SupplierPrice.UnitPrice = line.CostPrice;
                }
                if (line != null && line.ItemQuantity == 0 && goodReceptNoteDetail.FreeItemQuantity > 0)
                {
                line.CostPrice = line.CostPrice;
                line.VATPrecentage = goodReceptNoteDetail.VATPrecentage;
                }
            
        }
        public virtual void UpdateNbt(GoodReceptNoteDetail goodReceptNoteDetail)
        {
            GoodReceptNoteDetail line = lineCollection
            .Where(p => p.Product.ProductId == goodReceptNoteDetail.ProductId && p.SupplierPriceId == goodReceptNoteDetail.SupplierPriceId && p.ProductExpiryDate == goodReceptNoteDetail.ProductExpiryDate).FirstOrDefault();

            if (line != null && line.ItemQuantity > 0)
            {
                line.CostPrice = line.CostPrice * (1 + goodReceptNoteDetail.NBTPrecentage / 100);
                line.NBTPrecentage = goodReceptNoteDetail.NBTPrecentage;
                line.SupplierPrice.UnitPrice = line.CostPrice;
            }
            if (line != null && line.ItemQuantity == 0 && goodReceptNoteDetail.FreeItemQuantity > 0)
            {
                line.SupplierPrice.UnitPrice = line.CostPrice;
                line.NBTPrecentage = goodReceptNoteDetail.NBTPrecentage;

            }
        }
        public virtual void UpdateGroupDiscount(GoodReceptNoteDetail goodReceptNoteDetail)
        {
            GoodReceptNoteDetail line = lineCollection
            .Where(p => p.Product.ProductId == goodReceptNoteDetail.ProductId && p.SupplierPriceId == goodReceptNoteDetail.SupplierPriceId && p.ProductExpiryDate == goodReceptNoteDetail.ProductExpiryDate).FirstOrDefault();

            if (line != null && line.ItemQuantity > 0 && goodReceptNoteDetail.GroupDiscountAmount< 100)
            {
                line.GroupDiscountAmount = goodReceptNoteDetail.GroupDiscountAmount;
                line.SupplierPrice.UnitPrice = (line.CostPrice * line.ItemQuantity) * (1 /*- line.GroupDiscountAmount / 100*/) / (line.ItemQuantity); // remove + line.FreeItemQuantity
            }

        }
        public virtual void OverWriteStockPrice(GoodReceptNoteDetail goodReceptNoteDetail)
        {
            GoodReceptNoteDetail line = lineCollection
            .Where(p => p.Product.ProductId == goodReceptNoteDetail.ProductId && p.SupplierPriceId == goodReceptNoteDetail.SupplierPriceId && p.ProductExpiryDate == goodReceptNoteDetail.ProductExpiryDate).FirstOrDefault();

            if (line != null && line.ItemQuantity > 0)
            {
                line.overWriteStockPrice = goodReceptNoteDetail.overWriteStockPrice;
            }

        }
        public virtual void UpdateFreeQuantity(GoodReceptNoteDetail goodReceptNoteDetail)
        {
            GoodReceptNoteDetail line = lineCollection
            .Where(p => p.Product.ProductId == goodReceptNoteDetail.ProductId && p.SupplierPriceId == goodReceptNoteDetail.SupplierPriceId && p.ProductExpiryDate == goodReceptNoteDetail.ProductExpiryDate).FirstOrDefault();

            if (line != null && line.ItemQuantity > 0)
            {
                line.FreeItemQuantity = goodReceptNoteDetail.FreeItemQuantity;
                line.SupplierPrice.UnitPrice = ((line.CostPrice * line.ItemQuantity) /*- (line.GroupDiscountAmount * line.CostPrice * line.ItemQuantity)*/ ) / (line.ItemQuantity); // remove + line.FreeItemQuantity
                // line.CostPrice = (line.CostPrice * line.ItemQuantity) / (line.ItemQuantity + line.FreeItemQuantity);
            }
            if (line != null && line.ItemQuantity == 0 && goodReceptNoteDetail.FreeItemQuantity > 0)
            {
                line.FreeItemQuantity = goodReceptNoteDetail.FreeItemQuantity;
                line.SupplierPrice.UnitPrice = line.CostPrice;
                // line.CostPrice = (line.CostPrice * line.ItemQuantity) / (line.ItemQuantity + line.FreeItemQuantity);
            }
        }
        public virtual void RemoveLine(GoodReceptNoteDetail goodReceptNoteDetail) =>
        lineCollection.RemoveAll(l => l.Product.ProductId == goodReceptNoteDetail.ProductId && l.SupplierPriceId == goodReceptNoteDetail.SupplierPriceId && l.ProductExpiryDate == goodReceptNoteDetail.ProductExpiryDate);

        public virtual decimal? ComputeGrandTotalValue => lineCollection.Where(c => c.ItemQuantity > 0).Sum(e => e.CostPrice * e.ItemQuantity);//remove + e.FreeItemQuantity

        public virtual decimal? ComputeDiscountValue => lineCollection.Where(c => c.ItemQuantity > 0).Sum(e => (e.CostPrice * e.ItemQuantity) * (e.GroupDiscountAmount / 100) /*- e.SupplierPrice.UnitPrice * (e.ItemQuantity)*/);//remove + e.FreeItemQuantity

        public virtual decimal ComputeTotalValue => lineCollection.Where(c => c.ItemQuantity > 0).Sum(e => e.SupplierPrice.UnitPrice * (e.ItemQuantity) - (e.CostPrice * e.ItemQuantity) * (e.GroupDiscountAmount / 100));

        public virtual void Clear() => lineCollection.Clear();
        public virtual ICollection<GoodReceptNoteDetail> Lines => lineCollection;
        public virtual int Count => lineCollection.Count;

    }
}
