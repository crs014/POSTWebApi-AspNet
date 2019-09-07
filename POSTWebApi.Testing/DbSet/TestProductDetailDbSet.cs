using POSTWebApi.Models;
using System;
using System.Linq;

namespace POSTWebApi.Testing.DbSet
{
    public class TestProductDetailDbSet : TestDbSet<ProductDetail>
    {
        public override ProductDetail Find(params object[] keyValues)
        {
            return this.SingleOrDefault(productDetail => productDetail.Id == (Guid)keyValues.Single());
        }
    }
}
