using POSTWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSTWebApi.Testing.DbSet
{
    class TestRoleDbSet : TestDbSet<Role>
    {
        public override Role Find(params object[] keyValues)
        {
            return this.SingleOrDefault(role => role.Id == (Guid)keyValues.Single());
        }
    }
}
