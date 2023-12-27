using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Maliban.Pos.Api.Repository
{
  public class EFBrandRepository : RepositoryBase<Brand>, IBrandRepository
  {

    public EFBrandRepository(DataContext ctx) : base(ctx)
    {
      context = ctx;
    }
    public IQueryable<Brand> Brand => context.Brands;
    public async Task<PagedList<Brand>> GetBrandsAsync(UserParams userParams, bool status)
    {
      var brands = context.Brands.Where(b => b.Active == status).Include(p => p.Photo);
      return await PagedList<Brand>.CreateAsync(brands, userParams.PageNumber, userParams.PageSize);
    }

  }
}