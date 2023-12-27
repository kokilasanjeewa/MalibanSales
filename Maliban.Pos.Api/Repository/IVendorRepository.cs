using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public interface IVendorRepository: IRepositoryBase<Vendor>
  {
    IQueryable<Vendor> Vendors { get; }
    Task<PagedList<Vendor>> GetVendorsAsync(UserParams userParams);
  }
}