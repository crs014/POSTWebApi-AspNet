using POSTWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POSTWebApi.ViewModels.Entity
{
    public class PriceViewModel
    {
        public PriceViewModel() { }

        public PriceViewModel(Price price)
        {
            if(price != null)
            {
                Id = price.Id;
                Value = price.Value;
                DateTime = price.CreatedAt;
                RowVersion = price.RowVersion;
            }
        }

        public static IEnumerable<PriceViewModel> GetAll(IEnumerable<Price> prices)
        {
            return prices.Select(e => new PriceViewModel()
            {
                Id = e.Id,
                Value = e.Value,
                DateTime = e.CreatedAt,
                RowVersion = e.RowVersion
            });
        }

        public Guid Id { get; set; }

        public decimal Value { get; set; }

        public DateTime DateTime { get; set; }

        public byte[] RowVersion { get; set; }
    }
}