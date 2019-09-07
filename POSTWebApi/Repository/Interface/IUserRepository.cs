using POSTWebApi.Models;
using POSTWebApi.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POSTWebApi.Repository.Interface
{
    public interface IUserRepository
    {
        Task<User> Create(User data);

        Task<User> Delete(Guid id);

        Task<User> Get(Guid id);

        IEnumerable<User> GetAll(int skip, int limit);

        IEnumerable<User> GetAllDeleted(int skip, int limit);

        Task<User> SoftDelete(Guid id);

        Task<User> Update(Guid id, User data);

        Task<TokenViewModel> Login(LoginViewModel data);

        Task<TokenViewModel> RefreshToken(string data);

        Task<string> Logout(string token, Guid id);

        Task<string> LogoutAll(Guid userId);
    }
}
