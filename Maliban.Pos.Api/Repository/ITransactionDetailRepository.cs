using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public interface ITransactionDetailRepository : IRepositoryBase<TransactionDetail>
  {
    IQueryable<TransactionDetail> TransactionDetail { get; }

  }
}