using Kasir.DbContexts;
using Kasir.Model;
using Kasir.ModelLinker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasir.Services.ProductCreators
{
    public class DatabaseProductCreator: IProductCreator
    {
        private readonly iCassierDbContext context;
        public DatabaseProductCreator(iCassierDbContextFactory dbContextFactory)
        {
            context = dbContextFactory.CreateDbContext(Array.Empty<string>());
        }

        public DatabaseProductCreator(iCassierDbContext dbContext)
        {
            context = dbContext;
        }

        public List<Product> ConvertAll(List<ProductLnk> products)
        {
            return products.Select(x => ToProduct(x)).ToList();
        }

        public Product ToProduct(ProductLnk product)
        {
            int? categoryId = null;
            try
            {
                categoryId = context.Categories.First(x => x.Name == product.CategoryName).Id;
            }
            catch
            {
                throw new Exception($"There is no category with Name '{product.CategoryName}'");
            }
            return new Product()
            {
                Id = product.Id,
                Name = product.ProductName,
                Price = product.Price,
                Barcode = product.Barcode,
                PromoPrice = product.PromoPrice,
                Notes = product.Notes,
                CategoryID = categoryId ?? 0
            };
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
