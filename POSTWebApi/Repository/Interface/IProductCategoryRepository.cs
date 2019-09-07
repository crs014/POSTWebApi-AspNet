using POSTWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSTWebApi.Repository.Interface
{
    public interface IProductCategoryRepository
    {
        IEnumerable<ProductCategory> Create(IEnumerable<ProductCategory> productCategories);

        IEnumerable<ProductCategory> Delete(IEnumerable<Guid> productCategories, Guid productId);
    }
}
