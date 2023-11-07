using Kasir.DbContexts;
using Kasir.Model;
using Kasir.ModelLinker;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasir.Services.ProductProviders
{
    public class DatabaseProductProvider : IProductProvider, IDisposable
    {
        private readonly iCassierDbContext context;

        public DatabaseProductProvider(iCassierDbContextFactory dbContextFactory)
        {
            context = dbContextFactory.CreateDbContext(Array.Empty<string>());
        }

        public DatabaseProductProvider(iCassierDbContext dbContext)
        {
            context = dbContext;
        }

        public int CalculateStock(Product product)
        {
            int Income = ((IEnumerable<ReceiveItems>)context.ReceiveItems.Where(x => x.ProductID == product.Id)).Select(x => x.Quantity).Sum();
            int Outcome = ((IEnumerable<TransactionItem>) context.TransactionItems.Where(x => x.ProductID == product.Id)).Select(x => x.Qty).Sum();

            return Income - Outcome;
        }

        public async Task<IEnumerable<ProductLnk>> GetAllProducts()
        {
            IEnumerable<Product> products = await context.Products.ToListAsync();
            return products.Select(x => ToProductLnk(x));
        }

        public List<ProductLnk> ConvertAll(List<Product> products) {
            return products.Select(x => ToProductLnk(x)).ToList();
        }

        public Category? GetCategory(Product product)
        {
            Category? category = context.Categories.FirstOrDefault(x => x.Id == product.CategoryID);
            return category;
        }


        public ProductLnk ToProductLnk (Product product)
        {
            return new ProductLnk()
            {
                Id = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                Barcode = product.Barcode,
                PromoPrice = product.PromoPrice,
                Notes = product.Notes,
                CategoryName =GetCategory(product)?.Name ?? string.Empty,
                Stock = CalculateStock(product)
            };
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
