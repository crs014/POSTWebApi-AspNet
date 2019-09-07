using POSTWebApi.Models;
using System;
using System.Linq;

namespace POSTWebApi.Testing.DbSet
{
    public class TestRoleFeatureDbSet : TestDbSet<RoleFeature>
    {
        public override RoleFeature Find(params object[] keyValues)
        {
            return this.SingleOrDefault(RoleFeature => RoleFeature.RoleId == (Guid)keyValues.Single());
        }
    }
}
