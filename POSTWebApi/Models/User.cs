using POSTWebApi.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace POSTWebApi.Models
{

    public enum GenderEnum { Femaie, Male }

    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(255)]
        [Index(IsUnique = true)]
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [MaxLength(45)]
        [MinLength(6)]
        [Required]
        public string Password { get; set; }

        [MaxLength(45)]
        [Required]
        public string Name { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(45)]
        [Index(IsUnique = true)]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public GenderEnum Gender { get; set; }

        public byte[] Avatar { get; set; }

        [Timestamp]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte[] RowVersion { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<SaleRequest> SaleRequests { get; set; }

        public virtual ICollection<UserToken> UserTokens { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}