using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Maliban.Pos.Api.Repository
{
  internal class EFVendorRepository : RepositoryBase<Vendor>, IVendorRepository
  {

    public EFVendorRepository(DataContext ctx) : base(ctx)
    {
      context = ctx;
    }
    public IQueryable<Vendor> Vendors => context.Vendors;
    public async Task<PagedList<Vendor>> GetVendorsAsync(UserParams userParams)
    {
      var vendors = context.Vendors;
      return await PagedList<Vendor>.CreateAsync(vendors, userParams.PageNumber, userParams.PageSize);
    }
  }
}