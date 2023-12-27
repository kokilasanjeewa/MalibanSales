using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Maliban.Pos.Api.Repository
{
    public class EFShopStockRepository : RepositoryBase<ShopStock>, IShopStockRepository
    {
        private readonly ISortHelper<ShopStock> _sortHelper;
        private readonly ILog logger;

        public EFShopStockRepository(DataContext ctx, ISortHelper<ShopStock> sortHelper, ILog logger) : base(ctx)
        {
            context = ctx;
            _sortHelper = sortHelper;
            this.logger = logger;

        }
        /// <summary>
        /// Takes two numbers and multiplies them together, returning the result.
        /// </summary>
        /// <param name="a">The first number to multiply</param>
        /// <param name="b">The second number to multiply</param>
        /// <returns>The product of the two input numbers</returns>
        public async Task OverWriteStockDetailPriceAsync(Product product, int branchLocationId, Price price)
        {
            var stockdetails = await context.TransactionDetails.Where(s => s.BranchLocationId == branchLocationId && s.dQuantity - s.dIssueQuantity > 0).ToListAsync();
            foreach (var item in stockdetails)
            {
                if (item.ProductId == product.ProductId && item.BranchLocationId == branchLocationId)
                {
                    item.Price = price;
                    item.PriceId = price.PriceId;
                    item.dCostPrice = price.UnitPrice;
                    item.dSalePrice = price.SalePrice;
                    context.TransactionDetails.Update(item);
                    await context.SaveChangesAsync();
                }
            }
        }
        public async Task<PagedList<TransactionDetail>> GetStockDetailsAsync(UserParams userParams, int? branchLocationId)
        {
            var stocks = context.TransactionDetails.Where(s => s.BranchLocationId == branchLocationId && s.dQuantity - s.dIssueQuantity > 0).Include(p => p.Product).Include(p => p.Price).AsNoTracking();
            return await PagedList<TransactionDetail>.CreateAsync(stocks, userParams.PageNumber, userParams.PageSize);

        }
        public async Task<PagedList<ShopStock>> GetShopStocksAsync(UserParams userParams, int? branchLocationId)
        {
            var stocks = context.ShopStocks.Where(s => s.BranchLocationId == branchLocationId)
            .Include(p => p.Product).Include(p => p.Price).AsNoTracking();
            if (stocks.Count() > 0)
            {
                _sortHelper.ApplySort(ref stocks, userParams.OrderBy);
                //_sortHelper.SearchByName(ref stocks,x => x.SupplierName.ToLower().Contains(userParams.Filter.Trim().ToLower()));
                //SearchByName(ref suppliers, userParams.Filter);
                //ApplySort(ref suppliers, userParams.OrderBy);
            }
            return await PagedList<ShopStock>.CreateAsync(stocks, userParams.PageNumber, userParams.PageSize);
        }
        private void SearchByName(ref IQueryable<ShopStock> stocks, Int64 productId)
        {
            if (!stocks.Any())
                return;

            stocks = stocks.Where(o => o.ProductId == productId);
        }
        public async Task UpdateShopStockAsync(ShopStock shopStock)
        {
            if (await context.ShopStocks.AnyAsync(s => s.ProductId == shopStock.ProductId && s.BranchLocationId == shopStock.BranchLocationId))
            {
                var updateshopStock = await context.ShopStocks.Where(s => s.ProductId == shopStock.ProductId && s.BranchLocationId == shopStock.BranchLocationId).FirstOrDefaultAsync<ShopStock>();
                updateshopStock.UnitsInStock += shopStock.UnitsInStock;
                updateshopStock.ModifyDate = DateTime.Now;
                updateshopStock.ModifiedBy = shopStock.ModifiedBy;
                context.ShopStocks.Update(updateshopStock);
                await context.SaveChangesAsync();
            }
            if (!await context.ShopStocks.AnyAsync(s => s.ProductId == shopStock.ProductId && s.BranchLocationId == shopStock.BranchLocationId))
            {
                await context.ShopStocks.AddAsync(shopStock);
                await context.SaveChangesAsync();
            }

        }
        public async Task ModifyShopStockAsync(Int64 productId, int branchLocationId, decimal quantity, string modifiedBy)
        {
            if (await context.ShopStocks.AnyAsync(s => s.ProductId == productId && s.BranchLocationId == branchLocationId))
            {
                var updateshopStock = await context.ShopStocks.Where(s => s.ProductId == productId && s.BranchLocationId == branchLocationId).FirstOrDefaultAsync<ShopStock>();
                updateshopStock.UnitsInStock -= quantity;
                updateshopStock.ModifyDate = DateTime.Now;
                updateshopStock.ModifiedBy = modifiedBy;
                context.ShopStocks.Update(updateshopStock);
                await context.SaveChangesAsync();
            }
        }
        public async Task UpdateSaleTransaction(List<SaleDetail> array, int branchLocationId, string createdBy)
        {
            List<TransactionDetail> updateshopStock = new List<TransactionDetail>();
            //  var strategy = context.Database.CreateExecutionStrategy();
            //  await strategy.ExecuteAsync(async () =>
            //  {
            //  using (var transaction = context.Database.BeginTransaction())
            //  {
            try
            {
                foreach (var item in array)
                {
                    var price = await context.Prices.Where(s => s.PriceId == item.PriceId && s.BranchLocationId == branchLocationId).FirstOrDefaultAsync<Price>();
                    var product = await context.Products.Where(s => s.ProductId == item.ProductId).FirstOrDefaultAsync<Product>();

                    TransactionDetail transactionDetail = new TransactionDetail
                    {
                        dQuantity = item.Quantity,
                        ProductAvailable = true,
                        BranchLocationId = branchLocationId,
                        ProductId = item.ProductId,
                        PriceId = item.PriceId,
                        iDocID = item.SaleDetailId,
                        cDocNo = "INV" + item.SaleDetailId,
                        dDocDate = DateTime.Now,
                        cDocType = cDocType.INV,
                        cExtRefNum = "Sale Invoice Out",
                        dIssueQuantity = item.Quantity,
                        dNetQuantity = -item.Quantity,
                        dCostPrice = price.ModifiedUnitPrice,
                        dSalePrice = item.ItemPrice,
                        dValue = -item.Quantity * price.ModifiedUnitPrice,
                        dNetValue = 0,
                        dIN = 0,
                        dOUT = item.Quantity,
                        Barcode = product.Barcode,
                        CreatedBy = createdBy,
                        CreateDate = DateTime.Now,
                        ProductExpireDate = item.ProductExpireDate,
                        ProductName = product.ProductName
                    };
                    updateshopStock.Add(transactionDetail);
                }
                context.TransactionDetails.UpdateRange(updateshopStock);
                context.SaveChanges();
                //     transaction.Commit();

            }
            catch (Exception e)
            {
                //    transaction.Rollback();
                logger.Error(string.Join(",", array) + branchLocationId + createdBy + e.InnerException + e.Message + e.HelpLink + e.HResult + e.StackTrace);
            }
            //   }
            //});
        }
        public async Task UpdateStockAdjustmentTransaction(List<StockAdjustmentDetail> array, int branchLocationId, string createdBy)
        {
            List<TransactionDetail> updateshopStock = new List<TransactionDetail>();
            foreach (var item in array)
            {
                var price = await context.Prices.Where(s => s.PriceId == item.PriceId && s.BranchLocationId == branchLocationId).Select(p => new { p.ModifiedUnitPrice }).FirstOrDefaultAsync();
                var product = await context.Products.Where(s => s.ProductId == item.ProductId).Select(p=> new { p.ProductName,p.Barcode}).FirstOrDefaultAsync();

                if (item.AdjustmentType == "Out" || item.AdjustmentQuantity < 0)
                {
                    TransactionDetail transactionDetail = new TransactionDetail
                    {
                        dQuantity = -item.AdjustmentQuantity,
                        ProductAvailable = true,
                        BranchLocationId = branchLocationId,
                        ProductId = item.ProductId,
                        PriceId = item.PriceId,
                        iDocID = item.StockAdjustmentDetailId,
                        cDocNo = "ADJ" + item.StockAdjustmentDetailId,
                        dDocDate = DateTime.Now,
                        cDocType = cDocType.AOUT,
                        cExtRefNum = "Stock Adjustment Out",
                        dIssueQuantity = -item.AdjustmentQuantity,
                        dNetQuantity = item.AdjustmentQuantity,
                        dCostPrice = price.ModifiedUnitPrice,
                        dSalePrice = item.Price.SalePrice,
                        dValue = item.AdjustmentQuantity * price.ModifiedUnitPrice,
                        dNetValue = item.AdjustmentQuantity * price.ModifiedUnitPrice,
                        dIN = 0,
                        dOUT = item.AdjustmentQuantity,
                        Barcode = product.Barcode,
                        CreatedBy = createdBy,
                        ProductExpireDate = item.ProductExpireDate,
                        CreateDate = DateTime.Now,
                        ProductName = product.ProductName
                    };
                    updateshopStock.Add(transactionDetail);
                }
                if (item.AdjustmentType == "In" || item.AdjustmentQuantity > 0)
                {
                    TransactionDetail transactionDetail = new TransactionDetail
                    {
                        dQuantity = item.AdjustmentQuantity,
                        ProductAvailable = true,
                        BranchLocationId = branchLocationId,
                        ProductId = item.ProductId,
                        PriceId = item.PriceId,
                        iDocID = item.StockAdjustmentDetailId,
                        cDocNo = "ADJ" + item.StockAdjustmentDetailId,
                        dDocDate = DateTime.Now,
                        cDocType = cDocType.AIN,
                        cExtRefNum = "Stock Adjustment In",
                        dIssueQuantity = 0,
                        dNetQuantity = item.AdjustmentQuantity,
                        dCostPrice = price.ModifiedUnitPrice,
                        dSalePrice = item.Price.SalePrice,
                        dValue = item.AdjustmentQuantity * price.ModifiedUnitPrice,
                        dNetValue = item.AdjustmentQuantity * price.ModifiedUnitPrice,
                        dIN = item.AdjustmentQuantity,
                        dOUT = 0,
                        Barcode = product.Barcode,
                        ProductExpireDate = item.ProductExpireDate,
                        CreatedBy = createdBy,
                        CreateDate = DateTime.Now,
                        ProductName = product.ProductName
                    };
                    updateshopStock.Add(transactionDetail);
                }

            }
            context.TransactionDetails.UpdateRange(updateshopStock);
            await context.SaveChangesAsync();
        }
        public async Task UpdateSaleReturnTransaction(List<CustomerReturnDetail> array, int branchLocationId, string createdBy)
        {
            //var strategy = context.Database.CreateExecutionStrategy();
            //await strategy.ExecuteAsync(async () =>
            //{
            //    using (var transaction = context.Database.BeginTransaction())
            //    {
            //        try
            //        {
            List<TransactionDetail> updateshopStock = new List<TransactionDetail>();
            foreach (var item in array)
            {
                var price = await context.Prices.Where(s => s.PriceId == item.PriceId && s.BranchLocationId == branchLocationId).FirstOrDefaultAsync<Price>();
                TransactionDetail transactionDetail = new TransactionDetail
                {
                    dQuantity = item.ReturnQuantity,
                    ProductAvailable = true,
                    BranchLocationId = branchLocationId,
                    ProductId = item.ProductId,
                    PriceId = price.PriceId,
                    Price = price,
                    iDocID = item.CustomerReturnDetailId,
                    cDocNo = "SLR" + item.CustomerReturnDetailId,
                    dDocDate = DateTime.Now,
                    cDocType = cDocType.SLR,
                    cExtRefNum = "Sale Return",
                    dIssueQuantity = 0,
                    dNetQuantity = item.ReturnQuantity,
                    dCostPrice = price.ModifiedUnitPrice,
                    dSalePrice = price.SalePrice,
                    dValue = item.ReturnQuantity * price.ModifiedUnitPrice,
                    dNetValue = item.ReturnQuantity * price.ModifiedUnitPrice,
                    ProductExpireDate = item.ProductExpireDate,
                    dIN = item.ReturnQuantity,
                    dOUT = 0,
                    Barcode = item.Product.Barcode,
                    CreatedBy = createdBy,
                    CreateDate = DateTime.Now,
                    ProductName = item.Product.ProductName
                };
                updateshopStock.Add(transactionDetail);

            }
            context.TransactionDetails.UpdateRange(updateshopStock);
            await context.SaveChangesAsync();
            //            transaction.Commit();
            //        }
            //        catch (DbUpdateException e)
            //        {
            //            transaction.Rollback();
            //            logger.Error(e.InnerException + e.Message + e.HelpLink + e.HResult + e.StackTrace);
            //        }
            //        finally
            //        {
            //            transaction.Dispose();

            //        }
            //    }
            //});
        }
        public async Task UpdatePurchaseReturnTransaction(List<PurchaseReturnDetail> array, int branchLocationId, string createdBy)
        {
            //var strategy = context.Database.CreateExecutionStrategy();
            //await strategy.ExecuteAsync(async () =>
            //{
            //    using (var transaction = context.Database.BeginTransaction())
            //    {
            //        try
            //        {
            List<TransactionDetail> updateshopStock = new List<TransactionDetail>();
            foreach (var item in array)
            {
                var price = await context.Prices.Where(s => s.PriceId == item.PriceId && s.BranchLocationId == branchLocationId).FirstOrDefaultAsync<Price>();
                var product = await context.Products.Where(s => s.ProductId == item.ProductId).FirstOrDefaultAsync<Product>();

                TransactionDetail transactionDetail = new TransactionDetail
                {
                    dQuantity = item.ReturnQuantity,
                    ProductAvailable = true,
                    BranchLocationId = branchLocationId,
                    ProductId = item.ProductId,
                    PriceId = item.PriceId,
                    iDocID = item.PurchaseReturnDetailId,
                    cDocNo = "PRN" + item.PurchaseReturnDetailId,
                    dDocDate = DateTime.Now,
                    cDocType = cDocType.PRN,
                    cExtRefNum = "Purchase Return",
                    dIssueQuantity = item.ReturnQuantity,
                    dNetQuantity = -item.ReturnQuantity,
                    dCostPrice = price.ModifiedUnitPrice,
                    dSalePrice = item.Price.SalePrice,
                    dValue = item.ReturnQuantity * price.ModifiedUnitPrice,
                    ProductExpireDate = item.ProductExpireDate,
                    dNetValue = -item.ReturnQuantity * price.ModifiedUnitPrice,
                    dIN = 0,
                    dOUT = item.ReturnQuantity,
                    Barcode = product.Barcode,
                    CreatedBy = createdBy,
                    CreateDate = DateTime.Now,
                    ProductName = product.ProductName
                };
                updateshopStock.Add(transactionDetail);
            }
            context.TransactionDetails.UpdateRange(updateshopStock);
            await context.SaveChangesAsync();
            //transaction.Commit();

            //        catch (DbUpdateException e)
            //        {
            //            transaction.Rollback();
            //            logger.Error(e.InnerException + e.Message + e.HelpLink + e.HResult + e.StackTrace);
            //        }
            //        finally
            //        {
            //            transaction.Dispose();

            //        }
            //    }
            //});

        }
        public async Task<List<TransactionDetail>> ModifyStockDetailAsync(Int64 productId, int branchLocationId, decimal quantity, string modifiedBy, decimal dSalePrice, decimal dCostPrice)
        {
            decimal stockQty = await StockDetailQuantityAsync(productId, branchLocationId, dSalePrice, dCostPrice);
            var updateshopStocks = await context.TransactionDetails.Where(t => t.ProductId == productId && t.BranchLocationId == branchLocationId && t.dSalePrice == dSalePrice && t.dCostPrice == dCostPrice && t.dQuantity - t.dIssueQuantity > 0).ToListAsync<TransactionDetail>();
            if (await context.TransactionDetails.AnyAsync(s => s.ProductId == productId && s.BranchLocationId == branchLocationId && s.dSalePrice == dSalePrice && s.dCostPrice == dCostPrice) && quantity <= stockQty)
            {
                updateshopStocks = SubtractListArray(updateshopStocks, quantity);
            }
            return updateshopStocks;
        }
        public async Task<List<TransactionDetail>> ModifyStockDetailForPurchaseReturnAsync(Int64 productId, int branchLocationId, decimal quantity, string modifiedBy, decimal dSalePrice, decimal dCostPrice, Int64 TransactionDetailId)
        {
            decimal stockQty = await StockDetailQuantityAsync(productId, branchLocationId, dSalePrice, dCostPrice);
            var updateshopStocks = await context.TransactionDetails.Where(t => t.ProductId == productId && t.BranchLocationId == branchLocationId && t.dSalePrice == dSalePrice && t.dCostPrice == dCostPrice && t.dQuantity - t.dIssueQuantity > 0 && t.TransactionDetailId == TransactionDetailId).ToListAsync<TransactionDetail>();
            if (await context.TransactionDetails.AnyAsync(s => s.ProductId == productId && s.BranchLocationId == branchLocationId && s.dSalePrice == dSalePrice && s.dCostPrice == dCostPrice && s.TransactionDetailId == TransactionDetailId) && quantity <= stockQty)
            {
                updateshopStocks = SubtractListArray(updateshopStocks, quantity);
            }
            return updateshopStocks;
        }
        private List<TransactionDetail> SubtractListArray(List<TransactionDetail> array, decimal subtraction)
        {
            for (int i = 0; i < array.Count; i++)
            {
                if (subtraction > 0)
                {
                    decimal dNetQuantity = array[i].dQuantity - array[i].dIssueQuantity;
                    decimal newValue = dNetQuantity - subtraction;
                    array[i].dPreNetQuantity = dNetQuantity;
                    if (newValue < 1)
                    {
                        newValue = 0;
                        subtraction = subtraction - dNetQuantity;
                    }
                    dNetQuantity = newValue;
                    array[i].dIssueQuantity += array[i].dPreNetQuantity - dNetQuantity;
                    if (newValue > 1)
                    {
                        break;
                    }
                }
            }
            return array;
        }
        /* private List<TransactionDetail> SubtractListArray(List<TransactionDetail> array, decimal subtraction)
        {
            for (int i = 0; i < array.Count; i++)
            {
                if (subtraction > 0)
                {
                    decimal newValue = array[i].dNetQuantity - subtraction;
                    array[i].dPreNetQuantity = array[i].dNetQuantity;
                    if (newValue < 1)
                    {
                        newValue = 0;
                        subtraction = subtraction - array[i].dNetQuantity;
                    }
                    array[i].dNetQuantity = newValue;
                    array[i].dIssueQuantity += array[i].dPreNetQuantity - array[i].dNetQuantity;
                    if (newValue > 1)
                    {
                        break;
                    }
                }
            }
            return array;
        } */
        public async Task<decimal> StockQuantityAsync(Int64 productId, int branchLocationId)
        {
            var quantity = await context.ShopStocks.Where(s => s.ProductId == productId && s.BranchLocationId == branchLocationId).Select(st => new { st.UnitsInStock }).FirstOrDefaultAsync();
            if (quantity == null)
            {
                return 0;
            }
            return quantity.UnitsInStock;
        }
        public DateTime StockDetailQuantityWithExpiryDateAsync(Int64 productId, int branchLocationId, decimal SalePrice, decimal CostPrice)
        {
            var stocks = context.TransactionDetails.Where(s => s.ProductId == productId && s.BranchLocationId == branchLocationId && s.dSalePrice == SalePrice && s.dCostPrice == CostPrice && s.dQuantity - s.dIssueQuantity > 0)
            .GroupBy(p => new { p.ProductExpireDate }).Select(d => new { ProductExpireDate = d.Key.ProductExpireDate }).FirstOrDefault();

            if (stocks == null)
            {
                DateTime dt1 = new DateTime();
                return dt1.Date;
            }
            return stocks.ProductExpireDate;
        }
        public async Task<decimal> StockDetailQuantityAsync(Int64 productId, int branchLocationId, decimal SalePrice, decimal CostPrice)
        {
            var stocks = await context.TransactionDetails.Where(s => s.ProductId == productId && s.BranchLocationId == branchLocationId && s.dSalePrice == SalePrice && s.dCostPrice == CostPrice)
            .GroupBy(p => new { p.dSalePrice }).Select(p => new { Sum = p.Sum(t => t.dQuantity - t.dIssueQuantity) }).FirstOrDefaultAsync();
            if (stocks == null)
            {
                return 0;
            }
            return stocks.Sum;
        }
        public async Task<bool> ProductFindAsync(Int64 productId, int branchLocationId)
        {
            var product = await context.ShopStocks.Where(s => s.ProductId == productId && s.BranchLocationId == branchLocationId).Select(st => new { st.UnitsInStock }).FirstOrDefaultAsync();
            if (product == null) { return true; }
            return false;
        }
        public async Task UpdateEmployeeBalanceAsync(int MI_Employer_EmployeeId, decimal amount, string modifiedBy)
        {
            // var strategy = context.Database.CreateExecutionStrategy();
            // await strategy.ExecuteAsync(async () =>
            // {
            //  using (var transaction = context.Database.BeginTransaction())
            //   {
            //  {
            //try
            // {
            if (await context.MI_Employer_Employees.AnyAsync(s => s.MI_Employer_EmployeeId == MI_Employer_EmployeeId && s.Deleted == 0))
            {
                var updateemployeebalance = await context.MI_Employer_Employees.Where(e => e.MI_Employer_EmployeeId == MI_Employer_EmployeeId && e.Deleted == 0).FirstOrDefaultAsync<MI_Employer_Employee>();
                updateemployeebalance.PreviousCreditBalance = updateemployeebalance.CreditBalance;
                updateemployeebalance.CreditBalance += amount;
                //updateemployeebalance.ModifyDate = DateTime.Now;
                //updateemployeebalance.ModifiedBy = modifiedBy;
                context.MI_Employer_Employees.Update(updateemployeebalance);
                context.SaveChanges();
                //  transaction.Commit();
            }
            // }
            // catch (Exception ex)
            // {
            //  transaction.Rollback();
            // logger.Error(ex.InnerException + ex.Message + ex.HelpLink + ex.HResult + ex.StackTrace);
            // }

        }

        //    }
        // });
    }
}
