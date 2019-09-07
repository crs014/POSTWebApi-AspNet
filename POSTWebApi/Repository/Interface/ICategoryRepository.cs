using POSTWebApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POSTWebApi.Repository.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> Create(Category data);

        Task<Category> Delete(Guid id);

        Task<Category> Get(Guid id);

        IEnumerable<Category> GetAll(int skip, int limit);

        IEnumerable<Category> GetAllDeleted(int skip, int limit);

        Task<Category> SoftDelete(Guid id);

        Task<Category> Update(Guid id, Category data);
    }
}
