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
    public class ReceivedProductRepository : IReceivedProductRepository
    {
        private IDbPOS _db;

        public ReceivedProductRepository(IDbPOS db)
        {
            _db = db;
        }

        //plan tambah siapa penerima barang nya dan bikin sp
        public async Task<ReceivedProduct> Create(ReceivedProduct data)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    int totalQuantityProductDetail = _db.ProductDetails.Find(data.ProductDetailID).Quantity;
                    int totalReceivedProduct = 0;
                    var objReceivedProduct = _db.ReceivedProducts.Where(e => e.ProductDetailID == data.ProductDetailID);

                    if(objReceivedProduct.Count() != 0)
                    {
                        totalReceivedProduct = objReceivedProduct.Sum(e => e.Quantity);
                    }

                    if (totalQuantityProductDetail < totalReceivedProduct+data.Quantity)
                    {
                        transaction.Rollback();
                        throw new Exception("product is received more than request");
                    }

                    var receivedProductObj = await Task.Run(() =>
                    {
                        ReceivedProduct receivedProduct = _db.ReceivedProducts.Create();
                        receivedProduct.ProductDetailID = data.ProductDetailID;
                        receivedProduct.UserId = data.UserId;
                        receivedProduct.Quantity = data.Quantity;
                        _db.ReceivedProducts.Add(receivedProduct);
                        _db.SaveChanges();
                        transaction.Commit();
                        return receivedProduct;
                    });
                    return receivedProductObj;
                }
                catch(Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }
        }

        public Task<ReceivedProduct> Delete(Guid id)
        {
            return Task.Run(() =>
            {
                ReceivedProduct receivedProduct = _db.ReceivedProducts.Find(id);
                if(receivedProduct != null)
                {
                    _db.ReceivedProducts.Remove(receivedProduct);
                    _db.SaveChanges();
                }
                return receivedProduct;
            });
        }

        public IEnumerable<ReceivedProduct> Get(Guid id)
        {
            return _db.ReceivedProducts.Include(e => e.ProductDetail.Product)
                    .Where(e => e.ProductDetailID == id).OrderBy(e => e.CreatedAt);
        }

        public IEnumerable<ReceivedProduct> GetAll(int skip, int limit)
        {
            return _db.ReceivedProducts.Include(e => e.ProductDetail.Product)
                    .OrderBy(e => e.CreatedAt).Skip(skip).Take(limit);
        }

        public Task<ReceivedProduct> Update(Guid id, ReceivedProduct data)
        {
            return Task.Run(() =>
            {
                ReceivedProduct receivedProduct = _db.ReceivedProducts.Find(id);
                if (receivedProduct != null)
                {
                    receivedProduct.Quantity = receivedProduct.Quantity;
                    receivedProduct.ProductDetailID = receivedProduct.ProductDetailID;
                    receivedProduct.UpdatedAt = DateTime.UtcNow;
                    _db.SaveChanges();
                }
                return receivedProduct;
            });
        }
    }
}