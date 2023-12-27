using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Maliban.Pos.Api.Repository
{
  public class EFSubCategoryRepository : RepositoryBase<SubCategory>, ISubCategoryRepository
  {

    public EFSubCategoryRepository(DataContext ctx) : base(ctx)
    {
      context = ctx;
    }
    public IQueryable<SubCategory> SubCategory => context.SubCategories;
    public async Task<PagedList<SubCategory>> GetSubCategoriesAsync(UserParams userParams, bool status)
    {
      var subcategories = context.SubCategories.Where(b => b.Active == status);
      return await PagedList<SubCategory>.CreateAsync(subcategories, userParams.PageNumber, userParams.PageSize);
    }

  }
}