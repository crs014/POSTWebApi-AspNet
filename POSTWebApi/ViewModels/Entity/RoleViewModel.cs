using POSTWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POSTWebApi.ViewModels.Entity
{
    public class RoleViewModel
    {

        public RoleViewModel() { }

        public RoleViewModel(Role role)
        {
            Id = role.Id;
            Name = role.Name;
            RowVersion = role.RowVersion;
        }

        public static IEnumerable<RoleViewModel> GetAll(IEnumerable<Role> roles)
        {
            return roles.Select(e => new RoleViewModel()
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