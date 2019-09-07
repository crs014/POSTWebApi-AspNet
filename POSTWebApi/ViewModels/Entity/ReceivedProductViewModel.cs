using POSTWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POSTWebApi.ViewModels.Entity
{
    public class ReceivedProductViewModel
    {

        public ReceivedProductViewModel()
        {
        }

        public ReceivedProductViewModel(ReceivedProduct receivedProduct)
        {
            if(receivedProduct != null)
            {
                Id = receivedProduct.Id;
                ProductDetailID = receivedProduct.ProductDetailID;
                Quantity = receivedProduct.Quantity;
                RowVersion = receivedProduct.RowVersion;
                CreatedAt = receivedProduct.CreatedAt;
                UpdatedAt = receivedProduct.UpdatedAt;
                User = new UserViewModel(receivedProduct.User);
            }
        }

        public static IEnumerable<ReceivedProductViewModel> GetAll(IEnumerable<ReceivedProduct> receivedProducts)
        {
            return receivedProducts.Select(e => new ReceivedProductViewModel()
            {
                Id = e.Id,
                ProductDetailID = e.ProductDetailID,
                Quantity = e.Quantity,
                RowVersion = e.RowVersion,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt
            });
        }

        public Guid Id { get; set; }

        public Guid ProductDetailID { get; set; }

        public int Quantity { get; set; }

        public byte[] RowVersion { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public UserViewModel User { get; set; }
    }
}