//https://www.roundthecode.com/dotnet/asp-net-core-web-api/create-crud-api-endpoints-with-asp-net-core-entity-framework
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;

namespace Maliban.Pos.Api.Repository
{
  public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class, new()
  {
    protected DataContext context { get; set; }
    public RepositoryBase(DataContext ctx)
    {
      this.context = ctx;
    }
    public IQueryable<T> FindAll()
    {
      return this.context.Set<T>().AsNoTracking();
    }
    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
    {
      return this.context.Set<T>().Where(expression).AsNoTracking();
    }
    public async Task CreateAsync(T entity)
    {
      await this.context.Set<T>().AddAsync(entity);
    }
    public void Update(T entity)
    {
      this.context.Set<T>().Update(entity);

    }
/*     public virtual async Task<T> ReadAsync(int id, bool tracking = true)
    {
      var query =  this.context.Set<T>().AsQueryable();

      if (!tracking)
      {
        query = query.AsNoTracking();
      }

      return await query.FirstOrDefaultAsync(entity => entity.Id == id && !entity.Deleted.HasValue);
    } */
/*     public virtual async Task<T> UpdateAsync(int id, T updateEntity)
    {
      // Check that the record exists.
      var entity = await ReadAsync(id);

      if (entity == null)
      {
        throw new Exception("You've unable to find record with id '" + id + "'.");
      }

      // Update changes if any of the properties have been modified.
      this.context.Entry(entity).CurrentValues.SetValues(updateEntity);
      this.context.Entry(entity).State = EntityState.Modified;

      if (this.context.Entry(entity).Properties.Any(property => property.IsModified))
      {
        await this.context.SaveChangesAsync();
      }
      return entity;
    } */
    public void Delete(T entity)
    {
      this.context.Set<T>().Remove(entity);
    }
    public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
    {
      return await this.context.Set<T>().AnyAsync(expression);
    }
    public Expression<Func<T, object>> OrderBy { get; private set; }
    public Expression<Func<T, object>> OrderByDescending { get; private set; }

  }
}