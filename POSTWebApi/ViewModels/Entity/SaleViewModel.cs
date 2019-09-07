using POSTWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POSTWebApi.ViewModels.Entity
{
    public class SaleViewModel
    {
        public SaleViewModel()
        {

        }

        public SaleViewModel(Sale sale)
        {
            if(sale != null)
            {
                Id = sale.Id;
                RowVersion = sale.RowVersion;
                CreatedAt = sale.CreatedAt;
                UpdatedAt = sale.UpdatedAt;
                User = new UserViewModel(sale.User);
            }
        }

        public static IEnumerable<SaleViewModel> GetAll(IEnumerable<Sale> sales)
        {
            return sales.Select(e => new SaleViewModel()
            {
                Id = e.Id,
                RowVersion = e.RowVersion,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt,
                User = new UserViewModel(e.User)
            });
        }

        public Guid Id { get; set; }

        public byte[] RowVersion { get; set; }

        public UserViewModel User { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

    }
}