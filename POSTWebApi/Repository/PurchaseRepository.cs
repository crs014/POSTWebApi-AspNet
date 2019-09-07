using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using POSTWebApi.Models;
using POSTWebApi.Repository.Interface;
using System.Threading.Tasks;
using POSTWebApi.Services.Interfaces;

namespace POSTWebApi.Repository
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private IDbPOS _db;

        public PurchaseRepository(IDbPOS db)
        {
            _db = db;
        }

        public async Task<Purchase> Create(Purchase data)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var purchaseObj = await Task.Run(() =>
                    {
                        Purchase purchase = _db.Purchases.Create();
                        purchase.UserId = data.UserId;
                        purchase.SupplierId = data.SupplierId;
                        purchase.Supplier = _db.Suppliers.Find(data.SupplierId);
                        _db.Purchases.Add(purchase);
                        _db.SaveChanges();
                        _db.ProductDetails.AddRange(data.ProductDetails.Select(e =>
                        {
                            e.PurchaseId = purchase.Id;
                            return e;
                        }));
                        _db.SaveChanges();
                        transaction.Commit();
                        return purchase;
                    });
                    return purchaseObj;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }
        }

        public Task<Purchase> Delete(Guid id)
        {
            return Task.Run(() =>
            {
                Purchase purchase = _db.Purchases.Where(e => e.Id == id)
                        .Include(e => e.Supplier)
                        .Include(e => e.User.UserRoles.Select(a => a.Role)).FirstOrDefault();
                if (purchase != null)
                {
                    _db.Purchases.Remove(purchase);
                    _db.SaveChanges();
                }
                return purchase;
            });
        }

        public Task<Purchase> Get(Guid id)
        {
            try
            {
                return Task.Run(() =>
                {
                    return _db.Purchases
                      .Include(e => e.ProductDetails.Select(a => a.Product.Prices))
                      .Include(e => e.ProductDetails.Select(a => a.ReceivedProducts))
                      .Include(e => e.Supplier)
                      .Include(e => e.User.UserRoles.Select(a => a.Role))
                      .Where(e => e.Id == id)
                      .Where(e => e.DeletedAt == null).FirstOrDefault();
                });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<Purchase> GetAll(int skip, int limit)
        {
            return _db.Purchases.Where(e => e.DeletedAt == null).OrderBy(e => e.CreatedAt)
                    .Include(e => e.Supplier)
                    .Include(e => e.User.UserRoles.Select(a => a.Role))
                    .Skip(skip).Take(limit).ToList();
        }

        public IEnumerable<Purchase> GetAllDeleted(int skip, int limit)
        {
            return _db.Purchases.Where(e => e.DeletedAt != null).OrderBy(e => e.CreatedAt)
                    .Include(e => e.Supplier)
                    .Include(e => e.User.UserRoles.Select(a => a.Role))
                    .Skip(skip).Take(limit);
        }

        public Task<Purchase> SoftDelete(Guid id)
        {
            return Task.Run(() =>
            {
                Purchase purchase = _db.Purchases.Where(e => e.Id == id)
                                  .Include(e => e.Supplier)
                                  .Include(e => e.User.UserRoles.Select(a => a.Role))
                                  .Where(e => e.DeletedAt == null).FirstOrDefault();
                if (purchase != null)
                {
                    purchase.DeletedAt = DateTime.UtcNow;
                    _db.SaveChanges();
                }
                return purchase;
            });
        }

        public Task<Purchase> Update(Guid id, Purchase data)
        {
            return Task.Run(() =>
            {
                Purchase purchase = _db.Purchases.Where(e => e.Id == id)
                    .Include(e => e.Supplier)
                    .Include(e => e.User.UserRoles.Select(a => a.Role))
                    .Where(e => e.DeletedAt == null).FirstOrDefault();
                purchase.SupplierId = data.SupplierId;
                purchase.UserId = data.UserId;
                purchase.UpdatedAt = DateTime.UtcNow;
                _db.SaveChanges();
                return purchase;
            });
        }
    }
}