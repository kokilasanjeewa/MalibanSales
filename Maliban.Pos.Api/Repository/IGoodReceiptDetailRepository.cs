using System.Threading.Tasks;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public interface IGoodReceiptDetailRepository : IRepositoryBase<GoodReceptNoteDetail>
  {
    PagedList<GoodReceptNoteDetail> GetGoodReceiptDetailsAsync(UserParams userParams, int? goodReceptNoteHeaderId);
  }
}