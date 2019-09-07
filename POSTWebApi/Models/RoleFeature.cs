﻿using POSTWebApi.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace POSTWebApi.Models
{
    public class RoleFeature
    {
        [Key, Column(Order = 0)]
        public Guid FeatureId { get; set; }

        [Key, Column(Order = 1)]
        public Guid RoleId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Timestamp]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte[] RowVersion { get; set; }

        [ForeignKey("FeatureId")]
        public virtual Feature Feature { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
    }
}