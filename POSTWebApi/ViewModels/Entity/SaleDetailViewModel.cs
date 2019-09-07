using POSTWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POSTWebApi.ViewModels.Entity
{
    public class SaleDetailViewModel
    {
        public SaleDetailViewModel()
        {

        }

        public SaleDetailViewModel(Sale sale)
        {
            if(sale != null)
            {
                Id = sale.Id;
                RowVersion = sale.RowVersion;
                User = new UserViewModel(sale.User);
                CreatedAt = sale.CreatedAt;
                UpdatedAt = sale.UpdatedAt;

                if(sale.SaleDetails != null)
                {
                    ItemProducts = sale.SaleDetails.Select(e => new SaleDetailItemViewModel(e));
                }
            }
        }

        public Guid Id { get; set; }

        public byte[] RowVersion { get; set; }

        public UserViewModel User { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public IEnumerable<SaleDetailItemViewModel> ItemProducts { get; set; }
        

        public class SaleDetailItemViewModel
        {
            public SaleDetailItemViewModel(SaleDetail saleDetail)
            {
                if(saleDetail != null)
                {
                    Name = saleDetail.ProductDetail.Product.Name;
                    Description = saleDetail.ProductDetail.Product.Description;
                    CodeProduct = saleDetail.ProductDetail.Product.CodeProduct;
                    Quantity = saleDetail.Quantity;
                    DateTimeReceived = saleDetail.DateTimeReceived;
                    CreatedAt = saleDetail.CreatedAt;
                    UpdatedAt = saleDetail.UpdatedAt;
                    Expired = saleDetail.ProductDetail.Expired;
                    CodeProductDetail = saleDetail.ProductDetail.CodeProductDetail;
                    SalePrice = new PriceViewModel(saleDetail.ProductDetail.Product.Prices
                            .Where(a => a.Type == PriceTypeEnum.Sale)
                            .OrderByDescending(a => a.CreatedAt < CreatedAt).FirstOrDefault());
                }
            }

            public string Name { get; set; }

            public string Description { get; set; }

            public string CodeProduct { get; set; }

            public string CodeProductDetail { get; set; }

            public int Quantity { get; set; }

            public PriceViewModel SalePrice { get; set; }

            public DateTime? DateTimeReceived { get; set; }

            public DateTime? Expired { get; set; }

            public DateTime CreatedAt { get; set; }

            public DateTime UpdatedAt { get; set; }
        }
    }
}