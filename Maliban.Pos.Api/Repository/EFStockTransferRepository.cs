using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Dtos;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Maliban.Pos.Api.Repository
{
    public class EFStockTransferRepository : RepositoryBase<StockTransferDetail>, IStockTransferRepository
    {
        private readonly ISqlDataAccess _db;
        private readonly IDistributedCache _cache;
        private const string stockTransferListCacheKey = "stockTransferList";
        public EFStockTransferRepository(DataContext ctx, IDistributedCache cache, ISqlDataAccess db) : base(ctx)
        {
            context = ctx;
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _db = db;
        }
        public async Task<PagedList<StockTransferDetail>> GetStockTransferDetailAsync(UserParams userParams)
        {
            var stockTransferDetails = context.StockTransferDetails;
            return await PagedList<StockTransferDetail>.CreateAsync(stockTransferDetails, userParams.PageNumber, userParams.PageSize);
        }
        public async Task<PagedList<StockTransferHeader>> GetStockTransferHeaderAsync(UserParams userParams, int BranchLocationId)
        {
            var stockBranchHeaders = await context.StockTransferHeaders.Where(s => s.BranchLocationId == BranchLocationId).Include(p => p.PaymentMethod).AsNoTracking().ToListAsync();
            var stockTransferBranchHeaders =await context.StockTransferHeaders.Where(s => s.TransferBranchLocationId == BranchLocationId && s.cApprovalType== cApprovalType.Approve).Include(p => p.PaymentMethod).AsNoTracking().ToListAsync();
            List<StockTransferHeader> stockTransferHeaders = new List<StockTransferHeader>();
            foreach (var stockBranchHeader in stockBranchHeaders)
            {
                var transferBranch = context.BranchLocations.Where(s => s.BranchLocationId == stockBranchHeader.TransferBranchLocationId).AsNoTracking().FirstOrDefault();
                stockBranchHeader.TransferBranchLocation = transferBranch;
                stockBranchHeader.BranchLocation = null;
                stockBranchHeader.BranchLocation = context.BranchLocations.Where(s => s.BranchLocationId == stockBranchHeader.BranchLocationId).AsNoTracking().FirstOrDefault();

            }
            stockTransferHeaders.AddRange(stockBranchHeaders);

            foreach (var stockTransferBranchHeader in stockTransferBranchHeaders)
            {
                var transferBranch2 = context.BranchLocations.Where(s => s.BranchLocationId == stockTransferBranchHeader.TransferBranchLocationId).AsNoTracking().FirstOrDefault();
                stockTransferBranchHeader.TransferBranchLocation = transferBranch2;
                stockTransferBranchHeader.BranchLocation = null;
                stockTransferBranchHeader.BranchLocation = context.BranchLocations.Where(s => s.BranchLocationId == stockTransferBranchHeader.BranchLocationId).AsNoTracking().FirstOrDefault();
                stockTransferBranchHeader.cDocType = cDocType.STI;
            }
            stockTransferHeaders.AddRange(stockTransferBranchHeaders);
            return PagedList<StockTransferHeader>.CreateQAsync(stockTransferHeaders, userParams.PageNumber, userParams.PageSize);
        }
        public async Task<List<StockTransferDetail>> GetCacheStockTransferDetailAsync(int branchLocationId, string cacheKey)
        {
            var lineCollection = await _cache.GetValuesasync<List<StockTransferDetail>>(stockTransferListCacheKey);
            return lineCollection;
        }
        public async Task Remove()
        {
            await _cache.RemoveAsync(stockTransferListCacheKey);
        }
        public async Task RemoveLine(string lineId)
        {
            var lines = await _cache.GetValuesasync<List<StockTransferDetail>>(stockTransferListCacheKey);
            var cacheEntryOptions = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(600000)).SetAbsoluteExpiration(TimeSpan.FromSeconds(3600000));
            if (lines.Count > 0)
            {
                var line = lines.Where(c => c.StockTransferDetailUid == lineId).FirstOrDefault();
                lines.Remove(line);
                await _cache.RemoveAsync(stockTransferListCacheKey);
                await _cache.SetAsync(stockTransferListCacheKey, lines, cacheEntryOptions);
            }

        }
        public async Task<List<StockTransferDetail>> CreateStockTransferDetails(CreateStockTransferDetailDto stockTransferDetail)
        {
            decimal previouscartQty = 0;
            // configure cache option
            var cacheEntryOptions = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(600000)).SetAbsoluteExpiration(TimeSpan.FromSeconds(3600000));
            var lineCollection = await _cache.GetValuesasync<List<StockTransferDetail>>(stockTransferListCacheKey);

            if (await context.Products.AnyAsync(p => p.Barcode == stockTransferDetail.barCode) && stockTransferDetail.quantity > 0)
            {
                var itemProduct = await context.Products.Where(p => p.Barcode == stockTransferDetail.barCode).FirstOrDefaultAsync();
                // get cart stock in the given product 
                if (lineCollection != null)
                {
                    previouscartQty = lineCollection.Where(c => c.ProductId == itemProduct.ProductId && c.PriceId == stockTransferDetail.priceId).Sum(c => c.dTransferQuantity);
                    if (previouscartQty > 0)
                    {
                        foreach (var item in lineCollection)
                        {
                            var lines = lineCollection.Where(c => c.ProductId == item.ProductId && c.PriceId == item.PriceId).ToList();
                            foreach (var dupitem in lineCollection)
                            {
                                await RemoveLine(dupitem.StockTransferDetailUid);
                            }
                        }
                    }
                }
                var newlineCollection = await _cache.GetValuesasync<List<StockTransferDetail>>(stockTransferListCacheKey);

                // get available stock in the given product quantity
                List<StockTransferDetail> list = new List<StockTransferDetail>();
                var getAvailableStock = _db.LoadData<StockTransferDetail, dynamic>("dbo.sp_GetStockTransferDetailByLocation", new { xBranchlocation = stockTransferDetail.branchLocationId, xProductId = itemProduct.ProductId, xPriceId = stockTransferDetail.priceId, xQty = stockTransferDetail.quantity + previouscartQty }).Result.AsQueryable().ToList();
                foreach (var itemlist in getAvailableStock)
                {
                    list.Add(itemlist);
                }
                if (newlineCollection != null && newlineCollection.Count > 0)
                {
                    list.AddRange(newlineCollection);
                }

                // Add Item to List
                await _cache.SetAsync(stockTransferListCacheKey, list, cacheEntryOptions);
            }

            return await _cache.GetValuesasync<List<StockTransferDetail>>(stockTransferListCacheKey);
        }
        public List<ProductDetail> GetStockProducts(int branchLocationId, bool isProduct)
        {
            var getAvailableProduct = _db.LoadData<ProductDetail, dynamic>("dbo.sp_GetProductWithPriceByFIFO", new { xBranchlocation = branchLocationId, xProductId = 0, isProduct = isProduct }).Result.ToList();
            return getAvailableProduct.ToList();
        }
        public List<PriceDetail> GetPricesByStockProduct(int branchLocationId, Int64 productId)
        {
            var getAvailableProductPrices = _db.LoadData<PriceDetail, dynamic>("dbo.sp_GetProductWithPriceByFIFO", new { xBranchlocation = branchLocationId, xProductId = productId, isProduct = 0 }).Result.ToList();
            return getAvailableProductPrices.ToList();
        }
        public virtual decimal ComputeDiscountValue => _cache.GetValuesasync<List<StockTransferDetail>>(stockTransferListCacheKey).Result.Sum(e => (e.dCostPrice * 0) * (e.ApproveQuantity > 0 ? e.dTransferQuantity - e.ApproveQuantity : e.dTransferQuantity));
        public virtual decimal ComputeGrandTotalValue => _cache.GetValuesasync<List<StockTransferDetail>>(stockTransferListCacheKey).Result.Sum(e => (e.dCostPrice) * (e.ApproveQuantity > 0 ? e.dTransferQuantity - e.ApproveQuantity : e.dTransferQuantity));
        public virtual decimal ComputeTotalValue => _cache.GetValuesasync<List<StockTransferDetail>>(stockTransferListCacheKey).Result.Sum(e => (e.dCostPrice) * (e.ApproveQuantity > 0 ? e.dTransferQuantity - e.ApproveQuantity : e.dTransferQuantity));
        public virtual int Count => _cache.GetValuesasync<List<StockTransferDetail>>(stockTransferListCacheKey).Result.Count;
    }
    public class ProductDetail
    {
        public string Barcode { get; set; }
    }
    public class PriceDetail
    {
        public Int64 ProductId { get; set; }
        public Int64 PriceId { get; set; }
        public decimal dSalePrice { get; set; }
        public decimal dCostPrice { get; set; }
        public decimal MaximumRetailPrice { get; set; }
    }
}