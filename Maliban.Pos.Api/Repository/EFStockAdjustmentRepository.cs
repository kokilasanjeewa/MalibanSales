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
  public class EFStockAdjustmentRepository : RepositoryBase<SupplierPrice>, ISupplierPriceRepository
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISortHelper<SupplierPrice> _sortHelper;

    public EFStockAdjustmentRepository(DataContext ctx, ISortHelper<SupplierPrice> sortHelper, IHttpContextAccessor httpContextAccessor) : base(ctx)
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
        SearchByName(ref supplierPrices, userParams.Filter);
        //ApplySort(ref suppliers, userParams.OrderBy);
      }
      return await PagedList<SupplierPrice>.CreateAsync(supplierPrices, userParams.PageNumber, userParams.PageSize);
    }
    private void SearchByName(ref IQueryable<SupplierPrice> supplierPrices, string supplierName)
    {
      if (!supplierPrices.Any() || string.IsNullOrWhiteSpace(supplierName))
        return;

      supplierPrices = supplierPrices.Where(o => o.Supplier.SupplierName.ToLower().Contains(supplierName.Trim().ToLower()));
    }

  }
  //https://code-maze.com/sorting-aspnet-core-webapi/
}