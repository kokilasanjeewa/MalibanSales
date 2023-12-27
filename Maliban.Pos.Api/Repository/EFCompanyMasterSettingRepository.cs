using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Maliban.Pos.Api.Repository
{
  public class EFCompanyMasterSettingRepository : RepositoryBase<CalculationMaster>, ICompanyMasterSettingRepository
    {

    public EFCompanyMasterSettingRepository(DataContext ctx) : base(ctx)
    {
      context = ctx;
    }
    public IQueryable<CalculationMaster> CalculationMaster => context.CalculationMasters;
    public async Task<PagedList<CalculationMaster>> GetCalculationMastersAsync(UserParams userParams, bool status)
    {
      var brands = context.CalculationMasters.Where(b => b.Active == status);
      return await PagedList<CalculationMaster>.CreateAsync(brands, userParams.PageNumber, userParams.PageSize);
    }

  }
}