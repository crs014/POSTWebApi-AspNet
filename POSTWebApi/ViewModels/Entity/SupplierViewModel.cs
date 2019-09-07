using POSTWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POSTWebApi.ViewModels.Entity
{
    public class SupplierViewModel
    {
        public SupplierViewModel() { }

        public SupplierViewModel(Supplier supplier)
        {
            if (supplier != null)
            {
                Id = supplier.Id;
                Name = supplier.Name;
                Address = supplier.Address;
                Phone = supplier.Phone;
                Description = supplier.Description;
                RowVersion = supplier.RowVersion;
            }
        }

        public static IEnumerable<SupplierViewModel> GetAll(IEnumerable<Supplier> suppliers)
        {
            return suppliers.Select(e => new SupplierViewModel()
            {
                Id = e.Id,
                Name = e.Name,
                Address = e.Address,
                Phone = e.Phone,
                Description = e.Description,
                RowVersion = e.RowVersion
            });
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Description { get; set; }

        public byte[] RowVersion { get; set; }
    }
}