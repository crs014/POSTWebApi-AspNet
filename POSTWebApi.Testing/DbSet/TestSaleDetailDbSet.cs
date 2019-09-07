using POSTWebApi.Models;
using System;
using System.Linq;

namespace POSTWebApi.Testing.DbSet
{
    public class TestSaleDetailDbSet : TestDbSet<SaleDetail>
    {
        public override SaleDetail Find(params object[] keyValues)
        {
            return this.SingleOrDefault(saleDetail => saleDetail.ProductDetailId == (Guid)keyValues.Single());
        }
    }
}
