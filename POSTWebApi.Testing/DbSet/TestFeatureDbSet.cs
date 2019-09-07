using POSTWebApi.Models;
using System;
using System.Linq;

namespace POSTWebApi.Testing.DbSet
{
    public class TestFeatureDbSet : TestDbSet<Feature>
    {
        public override Feature Find(params object[] keyValues)
        {
            return this.SingleOrDefault(feature => feature.Id == (Guid)keyValues.Single());
        }
    }
}
