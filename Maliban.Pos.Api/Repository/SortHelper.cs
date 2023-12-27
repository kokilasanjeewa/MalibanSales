using System;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Linq;
using System.Collections.Generic;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
  public class SortHelper<T> : ISortHelper<T>
  {
    public IQueryable<T> SearchByName(ref IQueryable<T> entities, string searchTerm)
    {
      if (!entities.Any() || string.IsNullOrWhiteSpace(searchTerm))
        return entities;
      var orderParams = searchTerm.Trim().Split(',');
      var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
      var searchQueryBuilder = new StringBuilder();
      foreach (var param in orderParams)
      {
        if (string.IsNullOrWhiteSpace(param))
          continue;

        var propertyFromQueryName = param.Split("=")[0];
        var propertyFromQueryValue = param.Split("=")[1];
        var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

        if (objectProperty == null)
          continue;
        // searchQueryBuilder.Append($"{objectProperty.Name}.Contains({propertyFromQueryValue})");
        searchQueryBuilder.Append($"o => o.{objectProperty.Name}.Contains({propertyFromQueryValue}.Trim().ToLower())");

      }
      var searchQuery = searchQueryBuilder.ToString();
      //Where(o => o.SupplierName.ToLower().Contains(supplierName.Trim().ToLower()));
      //o => o.SupplierName.ToLower().Contains(supplierName.Trim().ToLower())
      return entities.Where(searchQuery);
    }

    public IQueryable<T> ApplySort(ref IQueryable<T> entities, string orderByQueryString)
    {
      if (!entities.Any())
        return entities;

      if (string.IsNullOrWhiteSpace(orderByQueryString))
      {
        return entities;
      }

      var orderParams = orderByQueryString.Trim().Split(',');
      var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
      var orderQueryBuilder = new StringBuilder();

      foreach (var param in orderParams)
      {
        if (string.IsNullOrWhiteSpace(param))
          continue;

        var propertyFromQueryName = param.Split(" ")[0];
        var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

        if (objectProperty == null)
          continue;

        var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";

        orderQueryBuilder.Append($"{objectProperty.Name} {sortingOrder}, ");
      }

      var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

      return entities.OrderBy(orderQuery);
    }

  }
}