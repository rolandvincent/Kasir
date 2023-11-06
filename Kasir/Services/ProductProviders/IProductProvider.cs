using Kasir.Model;
using Kasir.ModelLinker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasir.Services.ProductProviders
{
    public interface IProductProvider
    {
        Task<IEnumerable<ProductLnk>> GetAllProducts();
    }
}
