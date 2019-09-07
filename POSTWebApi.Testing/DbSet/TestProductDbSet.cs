using System;
using POSTWebApi.Models;
using System.Linq;

namespace POSTWebApi.Testing.DbSet
{
    class TestProductDbSet : TestDbSet<Product>
    {
        public override Product Find(params object[] keyValues)
        {
            return this.SingleOrDefault(product => product.Id == (Guid)keyValues.Single());
        }
    }
}
