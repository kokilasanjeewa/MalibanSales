using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Maliban.Pos.Api.Repository
{
  public class EFCategoryRepository : RepositoryBase<Category>,ICategoryRepository
  {

    public EFCategoryRepository(DataContext ctx):base(ctx)
    {
      context = ctx;
    }
    public IQueryable<Category> Category => context.Categories;
  
  public async Task<PagedList<Category>> GetCategoriesAsync(UserParams userParams, bool status)
    {
      var categories = context.Categories.Where(b => b.Active == status).Include(p => p.Photo);
      return await PagedList<Category>.CreateAsync(categories, userParams.PageNumber, userParams.PageSize);
    }

  }
}