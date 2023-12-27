using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Maliban.Pos.Api.Helper;
using System;
using System.Runtime.InteropServices;

namespace Maliban.Pos.Api.Repository
{
  public class EFPriceRepository : RepositoryBase<Price>, IPriceRepository
  {
    public EFPriceRepository(DataContext ctx) : base(ctx)
    {
      context = ctx;
    }
    public IQueryable<Price> Price => context.Prices;
    public async Task<bool> RemovePriceAsync(int? productId)
    {
      var price = await context.Prices.Where(p => p.Product.ProductId == productId).SingleOrDefaultAsync();
      price.IsDeleted = true;
      context.Prices.Update(price);
      return true;
    }
    public async Task DeleteAsync(Price price)
    {
      context.Prices.Remove(price);
      await context.SaveChangesAsync();
    }
    public async Task<Price> GetPriceAsync(Int64? productId, int? branchLocationId,[Optional] decimal salePrice)
    {
      Price price = await context.Prices.Where(p => p.IsDeleted == false && p.Active == true).FirstOrDefaultAsync(u => u.ProductId == productId && u.BranchLocationId == branchLocationId);
      if (salePrice > 0)
      {
        /*         var versionPrice = context.Prices.Where(p => p.IsDeleted == true && p.Active == false && p.ProductId == productId && p.BranchLocationId == branchLocationId).OrderByDescending(p => p.CreateDate).Take(1); */
        var versionPrice = context.Prices.Where(p => p.IsDeleted == false && p.Active == true && p.ProductId == productId && p.BranchLocationId == branchLocationId && p.SalePrice==salePrice).OrderByDescending(p => p.CreateDate).Take(1);
        Price pricev1 = new Price();
        foreach (var item in versionPrice)
        {
          pricev1 = item;
        }
        return pricev1;
      }
         
        return price;
    }
    public async Task<PagedList<Price>> GetPricesAsync(UserParams userParams)
    {
      var prices = context.Prices;
      return await PagedList<Price>.CreateAsync(prices, userParams.PageNumber, userParams.PageSize);
    }
    public async Task<PagedList<SupplierPrice>> GetSuplierPricesAsync(UserParams userParams)
    {
      var supplierprices = context.SupplierPrices;
      return await PagedList<SupplierPrice>.CreateAsync(supplierprices, userParams.PageNumber, userParams.PageSize);
    }

    public async Task<Price> UpdatePriceAsync(Price price)
    {
      if (await context.Prices.AnyAsync(s => s.ProductId == price.ProductId && s.BranchLocationId == price.BranchLocationId && s.Active == true && s.IsDeleted == false))
      {
        var updateprice = await context.Prices.Where(s => s.ProductId == price.ProductId && s.BranchLocationId == price.BranchLocationId && s.Active == true && s.IsDeleted == false && s.ModifiedUnitPrice == price.ModifiedUnitPrice.WithTwoDecimalPoints() && s.SalePrice == price.SalePrice.WithTwoDecimalPoints() && s.UnitPrice == price.UnitPrice.WithTwoDecimalPoints()).FirstOrDefaultAsync<Price>();

        if (updateprice != null)
        {
          updateprice.ModifiedDate = DateTime.Now;
          updateprice.ModifiedBy = price.ModifiedBy;
          try
          {
            context.Prices.Update(updateprice);
            await context.SaveChangesAsync();
          }
          catch (DbUpdateConcurrencyException e)
          {
            foreach (var entry in e.Entries)
            {
              if (entry.Entity is Price)
              {
                var proposedValues = entry.CurrentValues;
                var databaseValues = entry.GetDatabaseValues();

                foreach (var property in proposedValues.Properties)
                {
                  var proposedValue = proposedValues[property];
                  var databaseValue = databaseValues[property];
                  // TODO: decide which value should be written to database
                  // proposedValues[property] = <value to be saved>;
                }
                // Refresh original values to bypass next concurrency check
                entry.OriginalValues.SetValues(databaseValues);
              }
              else
              {
                throw new NotSupportedException(
                    "Don't know how to handle concurrency conflicts for "
                    + entry.Metadata.Name);
              }
            }
          }
          return updateprice;
        }
        await context.Prices.AddAsync(price);
        await context.SaveChangesAsync();
        return price;
      }
      if (!await context.Prices.AnyAsync(s => s.ProductId == price.ProductId && s.BranchLocationId == price.BranchLocationId && s.Active == true && s.IsDeleted == false))
        await context.Prices.AddAsync(price);
      await context.SaveChangesAsync();
      return price;
    }
    public async Task<decimal> CalculatePriceV1Async(decimal CostPrice, decimal MaximumRetailPrice, int branchLocationId)
    {
      var calculationMaster = await context.CalculationMasters.Where(c => c.BranchLocationId == branchLocationId && c.Active == true).FirstOrDefaultAsync<CalculationMaster>();

      decimal salePrice = CostPrice + Math.Round(((MaximumRetailPrice - CostPrice) / (calculationMaster.SalePricePrecentageCompany / 10)), MidpointRounding.AwayFromZero);
      return Math.Round(salePrice, MidpointRounding.AwayFromZero);
    }
    public decimal CalculatePriceAsync(decimal CostPrice, decimal MaximumRetailPrice, int branchLocationId, decimal salePricePrecentageCompany)
    {
      decimal s = (((MaximumRetailPrice - CostPrice)) * (100 - salePricePrecentageCompany)) / 100;
      /*       decimal salePrice = CostPrice + Math.Round(((MaximumRetailPrice - CostPrice) / (salePricePrecentageCompany / 10)), MidpointRounding.AwayFromZero); */
      decimal salePrice = CostPrice + s.WithTwoDecimalPoints();
      //  return Math.Round(salePrice, MidpointRounding.AwayFromZero);
      return salePrice;
    }
  }
}