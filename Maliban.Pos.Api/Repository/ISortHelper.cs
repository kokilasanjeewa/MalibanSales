using System;
using System.Linq;
using System.Linq.Expressions;

namespace Maliban.Pos.Api.Repository
{
  public interface ISortHelper<T>
  {
    IQueryable<T> ApplySort(ref IQueryable<T> entities, string orderByQueryString);
    IQueryable<T> SearchByName(ref IQueryable<T> entities,string searchTerm);
  }
}