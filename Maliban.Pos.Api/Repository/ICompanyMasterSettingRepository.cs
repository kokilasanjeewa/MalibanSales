using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public interface ICompanyMasterSettingRepository : IRepositoryBase<CalculationMaster>
  {
    IQueryable<CalculationMaster> CalculationMaster { get; }
    Task<PagedList<CalculationMaster>> GetCalculationMastersAsync(UserParams userParams,bool status);
  }
}