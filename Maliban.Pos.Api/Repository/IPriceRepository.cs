using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public interface IPriceRepository : IRepositoryBase<Price>
  {
    IQueryable<Price> Price { get; }
    Task<bool> RemovePriceAsync(int? productId);
    Task<PagedList<Price>> GetPricesAsync(UserParams userParams);
    Task<PagedList<SupplierPrice>> GetSuplierPricesAsync(UserParams userParams);
    Task<Price> GetPriceAsync(Int64? productId, int? branchLocationId, [Optional] decimal salePrice);
    Task<Price> UpdatePriceAsync(Price price);
    decimal CalculatePriceAsync(decimal CostPrice, decimal MaximumRetailPrice, int branchLocationId,decimal salePricePrecentageCompany);
    Task<decimal> CalculatePriceV1Async(decimal CostPrice, decimal MaximumRetailPrice, int branchLocationId);
  }
}