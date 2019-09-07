using POSTWebApi.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace POSTWebApi.Models
{
    public class SaleDetail
    {
        [Key, Column(Order = 0)]
        public Guid SaleId { get; set; }

        [Key, Column(Order = 1)]
        public Guid ProductDetailId { get; set; }

        [Range(1,10)]
        public int Quantity { get; set; }

        public DateTime? DateTimeReceived { get; set; }

        [ForeignKey("SaleId")]
        public virtual Sale Sale { get; set; }

        [ForeignKey("ProductDetailId")]
        public virtual ProductDetail ProductDetail { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}