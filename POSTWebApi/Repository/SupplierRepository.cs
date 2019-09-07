using POSTWebApi.Models;
using POSTWebApi.Repository.Interface;
using POSTWebApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSTWebApi.Repository
{
    public class SupplierRepository : ISupplierRepository
    {

        private IDbPOS _db;

        public SupplierRepository(IDbPOS db)
        {
            _db = db;
        }

        public Task<Supplier> Create(Supplier data)
        {
            return Task.Run(() =>
            {
                Supplier supplier = _db.Suppliers.Create();
                supplier.Name = data.Name;
                supplier.Address = data.Address;
                supplier.Phone = data.Phone;
                supplier.Description = data.Description;
                _db.Suppliers.Add(supplier);
                _db.SaveChanges();
                return supplier;
            });
        }

        public Task<Supplier> Delete(Guid id)
        {
            return Task.Run(() =>
            {
                Supplier supplier = _db.Suppliers.Find(id);
                if (supplier != null)
                {
                    _db.Suppliers.Remove(supplier);
                    _db.SaveChanges();
                }
                return supplier;
            });
       
        }

        public Task<Supplier> Get(Guid id)
        {
            return Task.Run(() =>
            {
                return _db.Suppliers.Where(e => e.Id == id)
                        .Where(e => e.DeletedAt == null).FirstOrDefault();
            });
        }

        public IEnumerable<Supplier> GetAll(int skip, int limit)
        {
            return _db.Suppliers.Where(e => e.DeletedAt == null)
                    .OrderByDescending(e => e.UpdatedAt)
                    .Skip(skip)
                    .Take(limit);
        }

        public IEnumerable<Supplier> GetAllDeleted(int skip, int limit)
        {
            return _db.Suppliers.Where(e => e.DeletedAt != null)
                    .OrderByDescending(e => e.UpdatedAt)
                    .Skip(skip)
                    .Take(limit);
        }

        public Task<Supplier> SoftDelete(Guid id)
        {
            return Task.Run(() =>
            {
                Supplier supplier = _db.Suppliers.Find(id);
                if (supplier != null)
                {
                    supplier.DeletedAt = DateTime.UtcNow;
                    _db.SaveChanges();
                }
                return supplier;
            });
         
        }

        public Task<Supplier> Update(Guid id, Supplier data)
        {
            return Task.Run(() =>
            {
                Supplier supplier = _db.Suppliers.Where(e => e.Id == id)
                .Where(e => e.DeletedAt == null).FirstOrDefault();
                if (supplier != null)
                {
                    supplier.Name = data.Name;
                    supplier.Address = data.Address;
                    supplier.Phone = data.Phone;
                    supplier.Description = data.Description;
                    supplier.UpdatedAt = DateTime.UtcNow;
                    _db.SaveChanges();
                }
                return supplier;
            });
            
        }
    }
}