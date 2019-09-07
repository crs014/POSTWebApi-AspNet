using POSTWebApi.Models;
using POSTWebApi.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using POSTWebApi.Services.Interfaces;

namespace POSTWebApi.Repository
{
    public class SaleRepository : ISaleRepository
    {
        private IDbPOS _db;

        public SaleRepository(IDbPOS db)
        {
            _db = db;
        }

        public async Task<Sale> Create(Sale data)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var saleObj = await Task.Run(() =>
                    {
                        IEnumerable<ProductDetail> productDetails = _db.ProductDetails.Include(e => e.ReceivedProducts);
                        IEnumerable<SaleDetail> saleDetails = _db.SaleDetails;

                        List<SaleDetail> saleDetailList = new List<SaleDetail>();
                        Sale sale = _db.Sales.Create();
                        sale.UserId = data.UserId;
                        _db.Sales.Add(sale);
                        _db.SaveChanges();

                        data.SaleDetails.ToList().ForEach((saleDetail) =>
                        {
                            var saleDetailItem = saleDetails.Where(e => e.ProductDetailId == saleDetail.ProductDetailId);
                            ProductDetail productDetail = productDetails.Where(e => e.Id == saleDetail.ProductDetailId).FirstOrDefault();
                            if(productDetail.ReceivedProducts.Count() != 0)
                            {
                                int countProductAlreadyOut = 0;
                                if(saleDetailItem.Count() != 0)
                                {
                                    countProductAlreadyOut = saleDetailItem.Sum(e => e.Quantity);
                                }

                                if (productDetail.ReceivedProducts.Sum(e => e.Quantity) >= saleDetail.Quantity + countProductAlreadyOut)
                                {
                                    saleDetail.SaleId = sale.Id;
                                    saleDetailList.Add(saleDetail);
                                }
                            }
                        });

                        if(saleDetailList.Count() == 0)
                        {
                            transaction.Rollback();
                            throw new Exception("Sale item is empty");
                        }

                        _db.SaleDetails.AddRange(saleDetailList);
                        _db.SaveChanges();
                        transaction.Commit();
                        return sale;
                    });
                    return saleObj;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }
        }                                                                                                                                                                                                                                                                                               

        public Task<Sale> Delete(Guid id)
        {
            return Task.Run(() =>
            {
                Sale sale = _db.Sales.Find(id);
                if(sale != null)
                {
                    _db.Sales.Remove(sale);
                    _db.SaveChanges();
                }
                return sale;
            });
        }

        public Task<Sale> Get(Guid id)
        {
            return Task.Run(() =>
            {
                return _db.Sales.Where(e => e.Id == id)
                       .Include(e => e.User.UserRoles.Select(a => a.Role))
                       .Include(e => e.SaleDetails.Select(a => a.ProductDetail.Product.Prices)).FirstOrDefault();
            });
        }

        public IEnumerable<Sale> GetAll(int skip, int limit)
        {
            return _db.Sales.Where(e => e.DeletedAt == null).OrderBy(e => e.CreatedAt)
                .Include(e => e.User.UserRoles.Select(a => a.Role))
                .Skip(skip).Take(limit);
        }

        public IEnumerable<Sale> GetAllDeleted(int skip, int limit)
        {
            return _db.Sales.Where(e => e.DeletedAt != null).OrderBy(e => e.CreatedAt)
                .Include(e => e.User.UserRoles.Select(a => a.Role))
                .Skip(skip).Take(limit);
        }

        public Task<Sale> SoftDelete(Guid id)
        {
            return Task.Run(() =>
            {
                Sale sale = _db.Sales.Where(e => e.Id == id)
                            .Include(e => e.User.UserRoles.Select(a => a.Role)).FirstOrDefault();
                if(sale != null)
                {
                    sale.DeletedAt = DateTime.UtcNow;
                    _db.SaveChanges();
                }
                return sale;
            });
        }

        public Task<Sale> Update(Guid id, Sale data)
        {
            return Task.Run(() =>
            {
                Sale sale = _db.Sales.Where(e => e.Id == id)
                    .Include(e => e.User.UserRoles.Select(a => a.Role))
                    .Where(e => e.DeletedAt == null).FirstOrDefault();
                sale.UserId = data.UserId;
                sale.UpdatedAt = DateTime.UtcNow;
                _db.SaveChanges();
                return sale;
            });
        }
    }
}