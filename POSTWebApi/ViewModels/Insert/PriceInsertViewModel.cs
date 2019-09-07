using System;
using System.ComponentModel.DataAnnotations;

namespace POSTWebApi.ViewModels.Insert
{
    public class PriceInsertViewModel
    {
        public Guid ProductId { get; set; }

        [Required]
        [Range(0, 1000000000)]
        public decimal Value { get; set; }
    }
}