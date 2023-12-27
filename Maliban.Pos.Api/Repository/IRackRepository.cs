using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public interface IRackRepository : IRepositoryBase<Rack>
  {
    IQueryable<Rack> Rack { get; }
    Task<PagedList<Rack>> GetRacksAsync(UserParams userParams);
  }
}