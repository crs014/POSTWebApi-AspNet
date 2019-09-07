using System;
using POSTWebApi.Models;
using System.Linq;

namespace POSTWebApi.Testing.DbSet
{
    class TestProductCategoryDbSet : TestDbSet<ProductCategory>
    {
        public override ProductCategory Find(params object[] keyValues)
        {
            return this.SingleOrDefault(product => product.ProductId == (Guid)keyValues.Single());
        }
    }
}
