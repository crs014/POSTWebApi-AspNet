using POSTWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POSTWebApi.ViewModels.Entity
{
    public class PurchaseDetailViewModel
    {
        public PurchaseDetailViewModel(Purchase purchase)
        {
            Id = purchase.Id;
            Supplier = new SupplierViewModel(purchase.Supplier);
            User = new UserViewModel(purchase.User);
            RowVersion = purchase.RowVersion;
            CreatedAt = purchase.CreatedAt;
            
            if(purchase.ProductDetails != null)
            {
                ProductsDetail = purchase.ProductDetails.Select(e => new ProductDetailViewModel(e));
            }
        }

        public Guid Id { get; set; }
        public SupplierViewModel Supplier { get; set; }
        public UserViewModel User { get; set; }
        public byte[] RowVersion { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<ProductDetailViewModel> ProductsDetail { get; set; }

        public class ProductDetailViewModel
        {
            public ProductDetailViewModel()
            {

            }

            public ProductDetailViewModel(ProductDetail productDetail)
            {
                if(productDetail != null)
                {
                    Id = productDetail.Id;
                    Product = new PurchaseProductViewModel(productDetail.Product, productDetail.CreatedAt, productDetail.ReceivedProducts);
                    Quantity = productDetail.Quantity;
                    CodeProductDetail = productDetail.CodeProductDetail;
                    Expired = productDetail.Expired;
                    RowVersion = productDetail.RowVersion;
                }
            }

            public Guid Id { get; set; }
            public PurchaseProductViewModel Product { get; set; }
            public int Quantity { get; set; }
            public string CodeProductDetail { get; set; }
            public DateTime? Expired { get; set; }
            public byte[] RowVersion { get; set; }


            public class PurchaseProductViewModel
            {
                public PurchaseProductViewModel()
                {

                }

                public PurchaseProductViewModel(Product product, DateTime createdAt, IEnumerable<ReceivedProduct> receivedProducts)
                {
                    Id = product.Id;
                    Name = product.Name;
                    Description = product.Description;
                    CodeProduct = product.CodeProduct;
                    PurchasePrice = new PriceViewModel(product.Prices
                        .Where(a => a.Type == PriceTypeEnum.Purchase)
                        .OrderByDescending(a => a.CreatedAt < createdAt).FirstOrDefault());

                    if (receivedProducts != null)
                    {
                        ReceivedProducts = ReceivedProductViewModel.GetAll(receivedProducts);
                    }
                }

                public Guid Id { get; set; }
                public string Name { get; set; }
                public string Description { get; set; }
                public string CodeProduct { get; set; }
                public PriceViewModel PurchasePrice { get; set; }
                public IEnumerable<ReceivedProductViewModel> ReceivedProducts { get; set; }
                public byte[] RowVersion { get; set; }

            }
        }

    }
}