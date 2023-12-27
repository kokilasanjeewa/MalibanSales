using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  internal class EFPhotoRepository : RepositoryBase<Photo>,IPhotoRepository
  {

    public EFPhotoRepository(DataContext ctx):base(ctx)
    {
      context = ctx;
    }
  }
}