using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace POSTWebApi.ViewModels.Insert
{
    public class ProductCategoryInsertViewModel
    {
        [Required(ErrorMessage = "Category is required")]
        public IEnumerable<Guid> Categories { get; set; }
    }
}