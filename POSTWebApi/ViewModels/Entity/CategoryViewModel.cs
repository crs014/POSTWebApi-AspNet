using POSTWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POSTWebApi.ViewModels.Entity
{
    public class CategoryViewModel
    {
        public CategoryViewModel(){}

        public CategoryViewModel(Category category)
        {
            Id = category.Id;
            Name = category.Name;
            RowVersion = category.RowVersion;
        }

        public static IEnumerable<CategoryViewModel> GetAll(IEnumerable<Category> categories)
        {
            return categories.Select(e => new CategoryViewModel()
            {
                Id = e.Id,
                Name = e.Name,
                RowVersion = e.RowVersion
            });
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public byte[] RowVersion { get; set; }
    }
}