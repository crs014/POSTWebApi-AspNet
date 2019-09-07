using POSTWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSTWebApi.Testing.DbSet
{
    class TestUserRoleDbSet : TestDbSet<UserRole>
    {
        public override UserRole Find(params object[] keyValues)
        {
            return this.SingleOrDefault(userRole => userRole.RoleId == (Guid)keyValues.Single());
        }
    }
}
