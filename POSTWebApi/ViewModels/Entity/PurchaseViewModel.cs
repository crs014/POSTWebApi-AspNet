using POSTWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POSTWebApi.ViewModels.Entity
{
    public class PurchaseViewModel
    {
        public PurchaseViewModel()
        {

        }

        public PurchaseViewModel(Purchase purchase)
        {
            Id = purchase.Id;
            Supplier = new SupplierViewModel(purchase.Supplier);
            User = new UserViewModel(purchase.User);
            RowVersion = purchase.RowVersion;
            CreatedAt = purchase.CreatedAt;
        }

        public static IEnumerable<PurchaseViewModel> GetAll(IEnumerable<Purchase> purchases)
        {
            return purchases.Select(e => new PurchaseViewModel()
            {
                Id = e.Id,
                Supplier = new SupplierViewModel(e.Supplier),
                User = new UserViewModel(e.User),
                RowVersion = e.RowVersion,
                CreatedAt = e.CreatedAt
            });
        }

        public Guid Id { get; set; }
        public SupplierViewModel Supplier { get; set; }
        public UserViewModel User { get; set; }
        public byte[] RowVersion { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}