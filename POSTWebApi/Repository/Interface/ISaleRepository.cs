using POSTWebApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POSTWebApi.Repository.Interface
{
    public interface ISaleRepository
    {
        Task<Sale> Create(Sale data);

        Task<Sale> Delete(Guid id);

        Task<Sale> Get(Guid id);

        IEnumerable<Sale> GetAll(int skip, int limit);

        IEnumerable<Sale> GetAllDeleted(int skip, int limit);

        Task<Sale> SoftDelete(Guid id);

        Task<Sale> Update(Guid id, Sale data);
    }
}
