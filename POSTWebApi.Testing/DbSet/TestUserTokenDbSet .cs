using POSTWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POSTWebApi.Testing.DbSet
{
    class TestUserTokenDbSet : TestDbSet<UserToken>
    {
        public override UserToken Find(params object[] keyValues)
        {
            return this.SingleOrDefault(token => token.Id == (Guid)keyValues.Single());
        }

        public override IEnumerable<UserToken> RemoveRange(IEnumerable<UserToken> entities)
        {
            return entities;
        }
    }
}
