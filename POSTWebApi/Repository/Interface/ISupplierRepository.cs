using POSTWebApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POSTWebApi.Repository.Interface
{
    public interface ISupplierRepository
    {
        Task<Supplier> Create(Supplier data);

        Task<Supplier> Delete(Guid id);

        Task<Supplier> Get(Guid id);

        IEnumerable<Supplier> GetAll(int skip, int limit);

        IEnumerable<Supplier> GetAllDeleted(int skip, int limit);

        Task<Supplier> SoftDelete(Guid id);

        Task<Supplier> Update(Guid id, Supplier data);
    }
}
