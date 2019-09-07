using POSTWebApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POSTWebApi.Repository.Interface
{
    public interface IProductRepository
    {
        Task<Product> Create(Product data);

        Task<Product> Delete(Guid id);

        Task<Product> Get(Guid id);

        IEnumerable<Product> GetAll(int skip, int limit);

        IEnumerable<Product> GetAllDeleted(int skip, int limit);

        Task<Product> SoftDelete(Guid id);

        Task<Product> Update(Guid id, Product data);
    }
}
