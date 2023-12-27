using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Maliban.Pos.Api.Repository
{
  public class EFTransactionDetailRepository : RepositoryBase<TransactionDetail>, ITransactionDetailRepository
  {
    public EFTransactionDetailRepository(DataContext ctx) : base(ctx)
    {
      context = ctx;
    }
    public IQueryable<TransactionDetail> TransactionDetail => context.TransactionDetails;

  }
}