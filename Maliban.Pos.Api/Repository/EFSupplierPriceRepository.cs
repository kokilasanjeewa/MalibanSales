using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using AutoMapper;
using Maliban.Pos.Api.Dtos;

namespace Maliban.Pos.Api.Repository
{
  public class EFSupplierPriceRepository : RepositoryBase<SupplierPrice>, ISupplierPriceRepository
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISortHelper<SupplierPrice> _sortHelper;
    public EFSupplierPriceRepository(DataContext ctx, ISortHelper<SupplierPrice> sortHelper, IHttpContextAccessor httpContextAccessor) : base(ctx)
    {
      context = ctx;
      _sortHelper = sortHelper;
      _httpContextAccessor = httpContextAccessor;
    }
    public async Task<PagedList<SupplierPrice>> GetSupplierPricesAsync(UserParams userParams, int? branchLocationId)
    {
      // get header data (on branch location id)
      string header = _httpContextAccessor.HttpContext.Request.Headers["BranchLocationId"];
      var supplierPrices = context.SupplierPrices.Where(sp => sp.BranchLocationId == branchLocationId)
      .Include(s => s.Supplier).Include(p => p.Product).AsQueryable();
      if (supplierPrices.Count() > 0)
      {
        _sortHelper.ApplySort(ref supplierPrices, userParams.OrderBy);
                //_sortHelper.SearchByName(ref suppliers,x => x.SupplierName.ToLower().Contains(userParams.Filter.Trim().ToLower()));
         if (!string.IsNullOrWhiteSpace(userParams.Filter))
         SearchByName(ref supplierPrices, userParams.Filter);
        //ApplySort(ref suppliers, userParams.OrderBy);
      }
      return await PagedList<SupplierPrice>.CreateAsync(supplierPrices, userParams.PageNumber, userParams.PageSize);
    }
    private void SearchByName(ref IQueryable<SupplierPrice> supplierPrices, string queryString)
    {
            var orderParams = queryString.Trim().Split(',');
            var orderParams1 = orderParams[0].Split(':');
            string supplierName = orderParams1[1];
            string barcode =orderParams[1].Split(':')[1];
  
                if (!supplierPrices.Any() || string.IsNullOrWhiteSpace(supplierName))
        return;
        if(!string.IsNullOrWhiteSpace(barcode) && string.IsNullOrWhiteSpace(supplierName))
            {
               supplierPrices = supplierPrices.Where(o => o.Product.Barcode.ToLower().Contains(barcode.Trim().ToLower())).Count() > 0 ? supplierPrices.Where(o => o.Product.Barcode.ToLower().Contains(barcode.Trim().ToLower())) : supplierPrices;
                return;
            }
            if(!string.IsNullOrWhiteSpace(barcode) && !string.IsNullOrWhiteSpace(supplierName))
            {
              supplierPrices = supplierPrices.Where(o => o.Supplier.SupplierName.ToLower().Contains(supplierName.Trim().ToLower()) && o.Product.Barcode.Contains(barcode)).Count() >0 ? supplierPrices.Where(o => o.Supplier.SupplierName.ToLower().Contains(supplierName.Trim().ToLower()) && o.Product.Barcode.Contains(barcode)) : supplierPrices;

            }
            supplierPrices = supplierPrices.Where(o => o.Supplier.SupplierName.ToLower().Contains(supplierName.Trim().ToLower())).Count() >0 ? supplierPrices.Where(o => o.Supplier.SupplierName.ToLower().Contains(supplierName.Trim().ToLower())): supplierPrices;

        }
    }
  //https://code-maze.com/sorting-aspnet-core-webapi/
}