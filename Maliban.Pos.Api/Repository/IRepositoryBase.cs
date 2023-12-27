using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Maliban.Pos.Api.Helper;

namespace Maliban.Pos.Api.Repository
{
  public interface IRepositoryBase<T> where T:class,new()
  {
    IQueryable<T> FindAll();
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
    Task CreateAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
    Expression<Func<T, object>> OrderBy { get; }
    Expression<Func<T, object>> OrderByDescending { get; }/* 
    Task<T> ReadAsync(int id, bool tracking = true); */
/* 
    Task<T> UpdateAsync(int id, T updateEntity); */
  }
}