using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public interface ICategoryRepository : IRepositoryBase<Category>
  {
    IQueryable<Category> Category { get; }
    Task<PagedList<Category>> GetCategoriesAsync(UserParams userParams, bool status);

  }
}