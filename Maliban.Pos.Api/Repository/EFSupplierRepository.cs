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

namespace Maliban.Pos.Api.Repository
{
  public class EFSupplierRepository : RepositoryBase<Supplier>, ISupplierRepository
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISortHelper<Supplier> _sortHelper;
    public EFSupplierRepository(DataContext ctx, ISortHelper<Supplier> sortHelper, IHttpContextAccessor httpContextAccessor) : base(ctx)
    {
      context = ctx;
      _sortHelper = sortHelper;
      _httpContextAccessor = httpContextAccessor;
    }
    public async Task<PagedList<Supplier>> GetSuppliersAsync(UserParams userParams, int? branchLocationId)
    {
      // get header data (on branch location id)
      string header = _httpContextAccessor.HttpContext.Request.Headers["BranchLocationId"];
      var suppliers = context.Suppliers.Where(s => s.BranchLocationId == branchLocationId)
      .Include(p => p.Photos).Include(p => p.City).Include(d => d.District).Include(t => t.Title)
      .Include(p => p.Provinces).Include(c => c.Country).AsNoTracking();
      if (suppliers.Count() > 0)
      {
        //_sortHelper.SearchByName(ref suppliers,userParams.Filter);
        _sortHelper.ApplySort(ref suppliers, userParams.OrderBy);
        SearchByName(ref suppliers, userParams.Filter);
        //ApplySort(ref suppliers, userParams.OrderBy);
      }
      return await PagedList<Supplier>.CreateAsync(suppliers, userParams.PageNumber, userParams.PageSize);
    }
    public async Task<Supplier> GetSupplierAsync(int? supplierId) => 
    await context.Suppliers.Where(s => s.SupplierId == supplierId)
      .Include(p => p.Photos).Include(p => p.City).Include(d => d.District).Include(t => t.Title)
      .Include(p => p.Provinces).Include(c => c.Country).SingleAsync<Supplier>();
   
    private void SearchByName(ref IQueryable<Supplier> suppliers, string supplierName)
    {
      if (!suppliers.Any() || string.IsNullOrWhiteSpace(supplierName))
        return;

      suppliers = suppliers.Where(o => o.SupplierName.ToLower().Contains(supplierName.Trim().ToLower()));
    }

  }
  //https://code-maze.com/sorting-aspnet-core-webapi/
}