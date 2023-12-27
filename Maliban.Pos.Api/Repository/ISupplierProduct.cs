using System.Linq;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public interface ISupplierProduct : IRepositoryBase<SupplierProduct>
  {
    IQueryable<SupplierProduct> SupplierProduct { get; }

  }
}