using System;
using POSTWebApi.Models;
using System.Linq;

namespace POSTWebApi.Testing.DbSet
{
    public class TestCategoryDbSet : TestDbSet<Category>
    {
        public override Category Find(params object[] keyValues)
        {
            return this.SingleOrDefault(category => category.Id == (Guid)keyValues.Single());
        }
    }
}
