using System;
using POSTWebApi.Models;
using System.Linq;

namespace POSTWebApi.Testing.DbSet
{
    public class TestSupplierDbSet : TestDbSet<Supplier>
    {
        public override Supplier Find(params object[] keyValues)
        {
            return this.SingleOrDefault(supplier => supplier.Id == (Guid)keyValues.Single());
        }
    }
}
