using System.Threading.Tasks;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public interface IGoodReceiptHeaderRepository : IRepositoryBase<GoodReceptNoteHeader>
  {
    Task<PagedList<GoodReceptNoteHeader>> GetGoodReceptNoteHeadersAsync(UserParams userParams, int? branchLocationId);
  }
}