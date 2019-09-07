using POSTWebApi.Models;
using System;
using System.Linq;

namespace POSTWebApi.Testing.DbSet
{
    public class TestPurchaseDbSet : TestDbSet<Purchase>
    {
        public override Purchase Find(params object[] keyValues)
        {
            return this.SingleOrDefault(purchase => purchase.Id == (Guid)keyValues.Single());
        }
    }
}
