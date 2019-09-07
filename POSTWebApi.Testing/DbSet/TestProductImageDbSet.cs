using POSTWebApi.Models;
using System;
using System.Linq;

namespace POSTWebApi.Testing.DbSet
{
    public class TestProductImageDbSet : TestDbSet<ProductImage>
    {
        public override ProductImage Find(params object[] keyValues)
        {
            return this.SingleOrDefault(productImg => productImg.Id == (Guid)keyValues.Single());
        }
    }
}
