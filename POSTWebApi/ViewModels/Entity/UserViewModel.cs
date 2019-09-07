using POSTWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POSTWebApi.ViewModels.Entity
{
    public class UserViewModel
    {
        public UserViewModel() { }

        public UserViewModel(User user)
        {
            if (user != null)
            {
                Id = user.Id;
                Email = user.Email;
                Name = user.Name;
                Phone = user.Phone;
                Gender = user.Gender;
                RowVersion = user.RowVersion;
                Roles = user.UserRoles.Select(a => new RoleViewModel(a.Role));
            }
        }

        public static IEnumerable<UserViewModel> GetAll(IEnumerable<User> users)
        {
            return users.Select(e => new UserViewModel()
            {
                Id = e.Id,
                Email = e.Email,
                Name = e.Name,
                Phone = e.Phone,
                Gender = e.Gender,
                RowVersion = e.RowVersion,
                Roles = e.UserRoles.Select(a => new RoleViewModel(a.Role))
            });
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public byte[] RowVersion { get; set; }
        public GenderEnum Gender { get; set; }
        public IEnumerable<RoleViewModel> Roles { get; set; }
    }
}