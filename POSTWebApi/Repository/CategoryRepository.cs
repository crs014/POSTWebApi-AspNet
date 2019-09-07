using POSTWebApi.Models;
using POSTWebApi.Repository.Interface;
using POSTWebApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSTWebApi.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private IDbPOS _db;

        public CategoryRepository(IDbPOS db)
        {
            _db = db;
        }
    
        public Task<Category> Create(Category data)
        {
            return Task.Run(() =>
            {
                Category category = _db.Categories.Create();
                category.Name = data.Name;
                _db.Categories.Add(category);
                _db.SaveChanges();
                return category;
            });
        }

        public Task<Category> Delete(Guid id)
        {
            return Task.Run(() =>
            {
                Category category = _db.Categories.Find(id);
                if (category != null)
                {
                    _db.Categories.Remove(category);
                    _db.SaveChanges();
                }
                return category;
            });  
        }

        public Task<Category> Get(Guid id)
        {
            return Task.Run(() => {
                return _db.Categories.Where(e => e.Id == id)
                    .Where(e => e.DeletedAt == null).FirstOrDefault();
            });
        }

        public IEnumerable<Category> GetAll(int skip, int limit)
        {
            return _db.Categories.Where(e => e.DeletedAt == null)
                .OrderBy(e => e.UpdatedAt)
                .Skip(skip)
                .Take(limit);
        }

        public IEnumerable<Category> GetAllDeleted(int skip, int limit)
        {
            return _db.Categories.Where(e => e.DeletedAt != null)
                    .OrderBy(e => e.UpdatedAt)
                    .Skip(skip)
                    .Take(limit);
        }

        public Task<Category> SoftDelete(Guid id)
        {
            return Task.Run(() =>
            {
                Category category = _db.Categories.Find(id);
                if (category != null)
                {
                    category.DeletedAt = DateTime.UtcNow;
                    _db.SaveChanges();
                }
                return category;
            });
        }

        public Task<Category> Update(Guid id, Category data)
        {
            return Task.Run(() =>
            {
                Category category = _db.Categories.Find(id);
                if (category != null)
                {
                    category.Name = data.Name;
                    category.UpdatedAt = DateTime.UtcNow;
                    _db.SaveChanges();
                }
                return category;
            });
        }
    }
}