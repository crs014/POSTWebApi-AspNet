using POSTWebApi.Models;
using System;
using System.Linq;

namespace POSTWebApi.Testing.DbSet
{
    public class TestReceivedProductDbSet : TestDbSet<ReceivedProduct>
    {
        public override ReceivedProduct Find(params object[] keyValues)
        {
            return this.SingleOrDefault(received => received.Id == (Guid)keyValues.Single());
        }
    }
}
