using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Maliban.Pos.Api.Repository
{
  public class EFCustomerMasterSettingRepository : RepositoryBase<CustomerMasterSetting>, ICustomerMasterSettingRepository
    {

    public EFCustomerMasterSettingRepository(DataContext ctx) : base(ctx)
    {
      context = ctx;
    }
    public IQueryable<CustomerMasterSetting> CustomerMasterSetting => context.CustomerMasterSettings;
    public async Task<PagedList<CustomerMasterSetting>> GetCustomerMasterSettingsAsync(UserParams userParams, bool status)
    {
      var brands = context.CustomerMasterSettings.Where(b => b.Active == status).Include(b => b.BranchLocation);
      return await PagedList<CustomerMasterSetting>.CreateAsync(brands, userParams.PageNumber, userParams.PageSize);
    }

  }
}