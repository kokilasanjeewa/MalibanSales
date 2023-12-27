using System.Linq;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Maliban.Pos.Api.Repository
{
  public class EFSupplierProduct : RepositoryBase<SupplierProduct>, ISupplierProduct
  {
    public EFSupplierProduct(DataContext ctx) : base(ctx)
    {
      context = ctx;
    }
    public IQueryable<SupplierProduct> SupplierProduct => context.SupplierProducts.Include(c => c.Product.Vendor);

  }
}