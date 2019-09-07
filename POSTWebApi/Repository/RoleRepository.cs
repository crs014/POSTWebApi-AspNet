using POSTWebApi.Models;
using POSTWebApi.Repository.Interface;
using POSTWebApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSTWebApi.Repository
{
    public class RoleRepository : IRoleRepository
    {

        private IDbPOS _db;

        public RoleRepository(IDbPOS db)
        {
            _db = db;
        }

        public Task<Role> Create(Role data)
        {
            return Task.Run(() =>
            {
                Role role = _db.Roles.Create();
                role.Name = data.Name;
                _db.Roles.Add(role);
                _db.SaveChanges();
                return role;
            });
        }

        public Task<Role> Delete(Guid id)
        {
            return Task.Run(() =>
            {
                Role role = _db.Roles.Find(id);
                if (role != null)
                {
                    _db.Roles.Remove(role);
                    _db.SaveChanges();
                }
                return role;
            });
           
        }

        public Task<Role> Get(Guid id)
        {
            return Task.Run(() =>
            {
                return _db.Roles.Where(e => e.Id == id)
                    .Where(e => e.DeletedAt == null).FirstOrDefault();
            });
        }

        public IEnumerable<Role> GetAll(int skip, int limit)
        {
            return _db.Roles.Where(e => e.DeletedAt == null)
                    .OrderBy(e => e.UpdatedAt)
                    .Skip(skip)
                    .Take(limit);
        }

        public IEnumerable<Role> GetAllDeleted(int skip, int limit)
        {
            return _db.Roles.Where(e => e.DeletedAt != null)
                    .OrderBy(e => e.UpdatedAt)
                    .Skip(skip)
                    .Take(limit);
        }

        public Task<Role> SoftDelete(Guid id)
        {
            return Task.Run(() =>
            {
                Role role = _db.Roles.Find(id);
                if (role != null)
                {
                    role.DeletedAt = DateTime.UtcNow;
                    _db.SaveChanges();
                }
                return role;
            });
        }

        public Task<Role> Update(Guid id, Role data)
        {
            return Task.Run(() =>
            {
                Role role = _db.Roles.Find(id);
                if (role != null)
                {
                    role.Name = data.Name;
                    role.UpdatedAt = DateTime.UtcNow;
                    _db.SaveChanges();
                }
                return role;
            });
           
        }
    }
}