using System.Linq;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
    public interface ISectionRepository: IRepositoryBase<Section>
    {
         IQueryable<Section> Section { get; }
    }
}