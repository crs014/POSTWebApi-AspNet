using POSTWebApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POSTWebApi.Repository.Interface
{
    public interface IRoleRepository
    {
        Task<Role> Create(Role data);

        Task<Role> Delete(Guid id);

        Task<Role> Get(Guid id);

        IEnumerable<Role> GetAll(int skip, int limit);

        IEnumerable<Role> GetAllDeleted(int skip, int limit);

        Task<Role> SoftDelete(Guid id);

        Task<Role> Update(Guid id, Role data);
    }
}
