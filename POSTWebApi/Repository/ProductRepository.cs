using POSTWebApi.Models;
using POSTWebApi.Repository.Interface;
using POSTWebApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace POSTWebApi.Repository
{
    public class ProductRepository : IProductRepository
    {
        private IDbPOS _db;

        public ProductRepository(IDbPOS db)
        {
            _db = db;
        }

        public Task<Product> Create(Product data)
        {
            return Task.Run(() =>
            {
                Product product = _db.Products.Create();
                product.Name = data.Name;
                product.Description = data.Description;
                product.CodeProduct = data.CodeProduct;
                product.ProductCategories = new List<ProductCategory>();
                product.ProductDetails = new List<ProductDetail>();
                product.Prices = new List<Price>();
                _db.Products.Add(product);
                _db.SaveChanges();
                return product;
            });
        }

        public Task<Product> Delete(Guid id)
        {
            return Task.Run(() =>
            {
                Product product = _db.Products.Where(e => e.Id == id)
                            .Include(e => e.ProductCategories
                            .Select(a => a.Category))
                            .Include(e => e.Prices)
                            .Include(e => e.ProductDetails.Select(a => a.SaleDetails)).FirstOrDefault();
                if (product != null)
                {
                    _db.Products.Remove(product);
                    _db.SaveChanges();
                }
                return product;
            });
        
        }

        public Task<Product> Get(Guid id)
        {
            return Task.Run(() =>
            {
                return _db.Products.Where(e => e.Id == id)
                    .Where(e => e.DeletedAt == null).Include(e => e.ProductCategories
                    .Select(a => a.Category))
                    .Include(e => e.Prices)
                    .Include(e => e.ProductDetails.Select(a => a.SaleDetails)).FirstOrDefault();
            });
        }

        public IEnumerable<Product> GetAll(int skip, int limit)
        {
            return _db.Products.Where(e => e.DeletedAt == null)
                    .Include(e => e.ProductCategories.Select(a => a.Category))
                    .Include(e => e.Prices)
                    .OrderBy(e => e.UpdatedAt)
                    .Skip(skip)
                    .Take(limit);
        }

        public IEnumerable<Product> GetAllDeleted(int skip, int limit)
        { 
            return _db.Products.Where(e => e.DeletedAt != null)
                    .Include(e => e.ProductCategories
                    .Select(a => a.Category))
                    .Include(e => e.Prices)
                    .Include(e => e.ProductDetails.Select(a => a.SaleDetails))
                    .OrderBy(e => e.UpdatedAt)
                    .Skip(skip)
                    .Take(limit);
        }

        public Task<Product> SoftDelete(Guid id)
        {
            return Task.Run(() =>
            {
                Product product = _db.Products.Where(e => e.Id == id)
                             .Where(e => e.DeletedAt == null).Include(e => e.ProductCategories
                             .Select(a => a.Category))
                             .Include(e => e.ProductDetails.Select(a => a.SaleDetails))
                             .Include(e => e.Prices).FirstOrDefault();
                if (product != null)
                {
                    product.DeletedAt = DateTime.UtcNow;
                    _db.SaveChanges();
                }
                return product;
            });
         
        }

        public Task<Product> Update(Guid id, Product data)
        {
            return Task.Run(() =>
            {
                Product product = _db.Products
                  .Include(e => e.ProductCategories.Select(a => a.Category))
                  .Include(e => e.ProductDetails.Select(a => a.SaleDetails))
                  .Include(e => e.Prices).Where(e => e.Id == id).FirstOrDefault();
                if (product != null)
                {
                    product.Name = data.Name;
                    product.Description = data.Description;
                    product.CodeProduct = data.CodeProduct;
                    product.UpdatedAt = DateTime.UtcNow;
                    _db.SaveChanges();
                }
                return product;
            });
        }
    }
}