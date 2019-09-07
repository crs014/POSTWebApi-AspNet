using POSTWebApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POSTWebApi.Repository.Interface
{
    public interface IPurchaseRepository
    {
        Task<Purchase> Create(Purchase data);

        Task<Purchase> Delete(Guid id);

        Task<Purchase> Get(Guid id);

        IEnumerable<Purchase> GetAll(int skip, int limit);

        IEnumerable<Purchase> GetAllDeleted(int skip, int limit);

        Task<Purchase> SoftDelete(Guid id);

        Task<Purchase> Update(Guid id, Purchase data);
    }
}
