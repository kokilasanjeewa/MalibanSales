using System.Threading.Tasks;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public interface IUserRepository : IRepositoryBase<ApplicationUser>
  {
    Task<PagedList<ApplicationUser>> GetUsersAsync(UserParams userParams, int? branchLocationId);
     Task<ApplicationUser> GetUserAsync(int userId);
  }
}