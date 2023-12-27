using System.Collections.Specialized;
using System.Linq;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public class EFSectionRepository : RepositoryBase<Section>, ISectionRepository
  {
    public EFSectionRepository(DataContext ctx) : base(ctx)
    {
      context = ctx;
    }
    public IQueryable<Section> Section => context.Sections;

  }


}