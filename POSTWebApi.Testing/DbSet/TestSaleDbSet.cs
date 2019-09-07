using POSTWebApi.Models;
using System;
using System.Linq;

namespace POSTWebApi.Testing.DbSet
{
    public class TestSaleDbSet : TestDbSet<Sale>
    {
        public override Sale Find(params object[] keyValues)
        {
            return this.SingleOrDefault(sale => sale.Id == (Guid)keyValues.Single());
        }
    }
}
