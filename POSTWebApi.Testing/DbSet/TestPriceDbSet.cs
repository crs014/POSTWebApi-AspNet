using System;
using POSTWebApi.Models;
using System.Linq;

namespace POSTWebApi.Testing.DbSet
{
    class TestPriceDbSet : TestDbSet<Price>
    {
        public override Price Find(params object[] keyValues)
        {
            return this.SingleOrDefault(price => price.Id == (Guid)keyValues.Single());
        }
    }
}
