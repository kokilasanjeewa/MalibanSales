using Maliban.Pos.Api.Dtos;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maliban.Pos.Api.Repository
{
    public interface IStockTransferRepository : IRepositoryBase<StockTransferDetail>
    {
        Task Remove();
        Task RemoveLine(string lineId);
        Task<PagedList<StockTransferDetail>> GetStockTransferDetailAsync(UserParams userParams);
        Task<PagedList<StockTransferHeader>> GetStockTransferHeaderAsync(UserParams userParams, int BranchLocationId);
        Task<List<StockTransferDetail>> CreateStockTransferDetails(CreateStockTransferDetailDto stockTransferDetail);
        List<ProductDetail> GetStockProducts(int branchLocationId,bool isProduct);
        List<PriceDetail> GetPricesByStockProduct(int branchLocationId, Int64 productId);
        decimal ComputeDiscountValue { get; }
        decimal ComputeGrandTotalValue { get; }
        decimal ComputeTotalValue { get;}
        int Count { get; }
        Task<List<StockTransferDetail>> GetCacheStockTransferDetailAsync(int branchLocationId,string cacheKey);


    }

}
