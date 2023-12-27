using System;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Models;
using Maliban.Pos.Api.Repository;
using Maliban.Pos.Api.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Maliban.Pos.Api.Helper
{
  public static class ServiceExtensions
  {
    public static void ConfigureRepositoryWrapper(this IServiceCollection services)
    {
      //https://github.com/CodeMazeBlog/advanced-rest-concepts-aspnetcore/blob/searching-end/Repository/OwnerRepository.cs
      services.AddScoped<ISortHelper<Supplier>, SortHelper<Supplier>>();
      services.AddScoped<ISortHelper<ApplicationUser>, SortHelper<ApplicationUser>>();
      services.AddScoped<ISortHelper<SupplierPrice>, SortHelper<SupplierPrice>>();
      services.AddScoped<ISortHelper<ShopStock>, SortHelper<ShopStock>>();
      services.AddScoped<ISortHelper<PurchaseRequestHeader>, SortHelper<PurchaseRequestHeader>>();
      services.AddScoped<ISortHelper<PurchaseRequestDetail>, SortHelper<PurchaseRequestDetail>>();
      services.AddScoped<ISortHelper<PurchaseOrderHeader>, SortHelper<PurchaseOrderHeader>>();
      services.AddScoped<ISortHelper<PurchaseOrderDetail>, SortHelper<PurchaseOrderDetail>>();
      services.AddScoped<ISortHelper<PurchaseReturnHeader>, SortHelper<PurchaseReturnHeader>>();
      services.AddScoped<ISortHelper<PurchaseReturnDetail>, SortHelper<PurchaseReturnDetail>>();
      services.AddScoped<ISortHelper<GoodReceptNoteDetail>, SortHelper<GoodReceptNoteDetail>>();
      services.AddScoped<ISortHelper<GoodReceptNoteHeader>, SortHelper<GoodReceptNoteHeader>>();
      services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
      //refactor dapper
      services.AddScoped<ISqlDataAccess, SqlDataAccess>();
    }
  }
  
}
