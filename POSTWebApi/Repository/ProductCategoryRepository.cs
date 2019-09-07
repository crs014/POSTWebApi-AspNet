using POSTWebApi.Models;
using POSTWebApi.Repository.Interface;
using POSTWebApi.Services;
using POSTWebApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POSTWebApi.Repository
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private IDbPOS _db;

        public ProductCategoryRepository(IDbPOS db)
        {
            _db = db;
        }

        public IEnumerable<ProductCategory> Create(IEnumerable<ProductCategory> productCategories)
        {
            _db.ProductCategories.Add(new ProductCategory());
            _db.ProductCategories.AddRange(productCategories);
            _db.SaveChanges();
            return productCategories;
        }

        public IEnumerable<ProductCategory> Delete(IEnumerable<Guid> productCategories, Guid productId)
        {
            var productCategoriesList = _db.ProductCategories
                .Where(e => e.ProductId == productId)
                .Where(e => productCategories.Contains(e.CategoryId)).ToList();
           

            _db.ProductCategories.RemoveRange(productCategoriesList);
            _db.SaveChanges();
            return productCategoriesList;
        }
    }
}