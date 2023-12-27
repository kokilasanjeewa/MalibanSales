using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;


namespace Maliban.Pos.Api.Repository
{
  public interface ISubCategoryRepository : IRepositoryBase<SubCategory>
  {
    IQueryable<SubCategory> SubCategory { get; }
    Task<PagedList<SubCategory>> GetSubCategoriesAsync(UserParams userParams, bool status);
  }
}