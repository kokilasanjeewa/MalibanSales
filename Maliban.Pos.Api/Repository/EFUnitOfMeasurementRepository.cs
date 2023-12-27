using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public class EFUnitOfMeasurementRepository : RepositoryBase<UnitOfMeasurement>, IUnitOfMeasurementRepository
  {

    public EFUnitOfMeasurementRepository(DataContext ctx) : base(ctx)
    {
      context = ctx;
    }
    public IQueryable<UnitOfMeasurement> UnitOfMeasurement => context.UnitOfMeasurements;
    public async Task<PagedList<UnitOfMeasurement>> GetUnitOfMeasurementsAsync(UserParams userParams)
    {
      var uoms = context.UnitOfMeasurements;
      return await PagedList<UnitOfMeasurement>.CreateAsync(uoms, userParams.PageNumber, userParams.PageSize);
    }
  }
}