using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public interface IProductRepository:IRepositoryBase<Product>
  {
    IQueryable<Product> Product { get; }
    Task<bool> RemoveProductAsync(string barcode);
    Task<PagedList<Product>> GetProductsAsync(UserParams userParams);
    Task<Product> GetProductAsync(string barcode);
    Task<List<Product>> GetProductsByNameAsync(string productName);

    }
}