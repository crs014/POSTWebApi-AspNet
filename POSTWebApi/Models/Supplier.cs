using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POSTWebApi.Models
{
    public class Supplier
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(45, ErrorMessage = "Name max length is 45")]
        public string Name { get; set; }

        [MaxLength(45, ErrorMessage = "Address max length is 45")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Format phone number is wrong")]
        public string Phone { get; set; }

        [MaxLength(45, ErrorMessage = "Description max length is 45")]
        public string Description { get; set; }

        [Timestamp]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte[] RowVersion { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}