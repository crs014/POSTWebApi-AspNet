using POSTWebApi.Models;
using System;
using System.Linq;

namespace POSTWebApi.Testing.DbSet
{
    public class TestSaleRequestDbSet : TestDbSet<SaleRequest>
    {
        public override SaleRequest Find(params object[] keyValues)
        {
            return this.SingleOrDefault(saleRequest => saleRequest.SaleId == (Guid)keyValues.Single());
        }
    }
}
