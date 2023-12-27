using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public interface ICustomerMasterSettingRepository : IRepositoryBase<CustomerMasterSetting>
  {
    IQueryable<CustomerMasterSetting> CustomerMasterSetting { get; }
    Task<PagedList<CustomerMasterSetting>> GetCustomerMasterSettingsAsync(UserParams userParams,bool status);
  }
}