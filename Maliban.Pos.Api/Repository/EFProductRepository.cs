using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Maliban.Pos.Api.Helper;
using System.Runtime.InteropServices;

namespace Maliban.Pos.Api.Repository
{
    public class EFProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public EFProductRepository(DataContext ctx) : base(ctx)
        {
            context = ctx;
        }
        public IQueryable<Product> Product => context.Products.Include(v => v.Vendor);
        public async Task<bool> RemoveProductAsync(string barcode)
        {
            var product = await context.Products.Where(p => p.Barcode == barcode).SingleOrDefaultAsync();
            product.IsDeleted = true;
            context.Products.Update(product);
            return true;
        }
        public async Task DeleteAsync(Product product)
        {
            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }
        public async Task<Product> GetProductAsync(string barcode)
        {
            Product product = await context.Products.FirstOrDefaultAsync(u => u.Barcode == barcode && u.IsDeleted == false);
            //.Include(p => p.Photos)
            ;
            return product;
        }
        public async Task<List<Product>> GetProductsByNameAsync(string productName)
        {
            List<Product> products = await context.Products.Where(p => p.IsDeleted == false && p.ProductName.ToLower().Contains(productName.Trim().ToLower()))
            .Include(p => p.Photos)
            .AsNoTracking()
            .ToListAsync();
            return products;
        }
        public async Task<PagedList<Product>> GetProductsAsync(UserParams userParams)
        {
            var products = context.Products.Include(p => p.Photos).Include(u => u.UnitOfMeasurement).Include(v => v.Vendor).AsNoTracking();
            if (products.Count() > 0)
            {
                SearchByCode(ref products, userParams.Filter);
            }
            return await PagedList<Product>.CreateAsync(products, userParams.PageNumber, userParams.PageSize);
        }
        private void SearchByCode(ref IQueryable<Product> products, string code)
        {
            if (!products.Any() || string.IsNullOrWhiteSpace(code))
                return;

            //products = products.Where(o => o.Barcode.ToLower().Contains(code.Trim().ToLower()) || o.ProductName.ToLower().Contains(code.Trim().ToLower()));
            products = products.Where(o => o.Barcode.ToLower().Contains(code.Trim().ToLower())).Count() > 0 ? products.Where(o => o.Barcode.ToLower().Contains(code.Trim().ToLower())) : products.Where(o => o.ProductName.ToLower().Contains(code.Trim().ToLower()));

        }
    }
}