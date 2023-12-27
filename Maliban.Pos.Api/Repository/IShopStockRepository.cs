using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public interface IShopStockRepository : IRepositoryBase<ShopStock>
  {
    Task ModifyShopStockAsync(Int64 productId, int branchLocationId, decimal quantity, string modifiedBy);

    Task<List<TransactionDetail>> ModifyStockDetailAsync(Int64 productId, int branchLocationId, decimal quantity, string modifiedBy, decimal dSalePrice,decimal dCostPrice);
    Task<List<TransactionDetail>> ModifyStockDetailForPurchaseReturnAsync(Int64 productId, int branchLocationId, decimal quantity, string modifiedBy, decimal dSalePrice, decimal dCostPrice,Int64 TransactionDetailId);
    Task UpdateShopStockAsync(ShopStock shopStock);
    Task<decimal> StockQuantityAsync(Int64 productId, int branchLocationId);
    Task<decimal> StockDetailQuantityAsync(Int64 productId, int branchLocationId, decimal SalePrice, decimal CostPrice);
    DateTime StockDetailQuantityWithExpiryDateAsync(Int64 productId, int branchLocationId, decimal SalePrice, decimal CostPrice);
    Task<bool> ProductFindAsync(Int64 productId, int branchLocationId);
    Task UpdateEmployeeBalanceAsync(int MI_Employer_EmployeeId, decimal amount, string modifiedBy);
   Task<PagedList<ShopStock>> GetShopStocksAsync(UserParams userParams, int? branchLocationId);
    Task<PagedList<TransactionDetail>> GetStockDetailsAsync(UserParams userParams, int? branchLocationId);
    Task UpdateSaleTransaction(List<SaleDetail> array, int branchLocationId, string createdBy);
    Task OverWriteStockDetailPriceAsync(Product product, int branchLocationId, Price price);
    Task UpdateStockAdjustmentTransaction(List<StockAdjustmentDetail> array, int branchLocationId, string createdBy);
    Task UpdatePurchaseReturnTransaction(List<PurchaseReturnDetail> array, int branchLocationId, string createdBy);
    Task UpdateSaleReturnTransaction(List<CustomerReturnDetail> array, int branchLocationId, string createdBy);
  }
}