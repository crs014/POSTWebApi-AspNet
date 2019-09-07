using POSTWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POSTWebApi.ViewModels.Entity
{
    public class ProductViewModel
    {

        public ProductViewModel()
        {
            
        }

        public ProductViewModel(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            CodeProduct = product.CodeProduct;
            RowVersion = product.RowVersion;
            Categories = product.ProductCategories.Select(e => new CategoryViewModel(e.Category));
            PurchasePrice = new PriceViewModel(
                product.Prices.Where(e => e.Type == PriceTypeEnum.Purchase).OrderByDescending(a => a.CreatedAt).FirstOrDefault());
            SalePrice = new PriceViewModel(
                product.Prices.Where(e => e.Type == PriceTypeEnum.Sale).OrderByDescending(a => a.CreatedAt).FirstOrDefault());
        }

        public static IEnumerable<ProductViewModel> GetAll(IEnumerable<Product> products)
        {
            return products.Select(e => new ProductViewModel()
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                CodeProduct = e.CodeProduct,
                RowVersion = e.RowVersion,
                Categories = e.ProductCategories.Select(a => new CategoryViewModel(a.Category)),
                PurchasePrice = new PriceViewModel(
                    e.Prices.Where(a => a.Type == PriceTypeEnum.Purchase).OrderByDescending(a => a.CreatedAt).FirstOrDefault()),
                SalePrice = new PriceViewModel(
                    e.Prices.Where(a => a.Type == PriceTypeEnum.Sale).OrderByDescending(a => a.CreatedAt).FirstOrDefault())
            });
        }


        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string CodeProduct { get; set; }

        public byte[] RowVersion { get; set; }
        
        public IEnumerable<CategoryViewModel> Categories { get; set; }

        public PriceViewModel SalePrice { get; set; }

        public PriceViewModel PurchasePrice { get; set; }
    }
}