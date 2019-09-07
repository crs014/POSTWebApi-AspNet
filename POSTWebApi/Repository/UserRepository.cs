using POSTWebApi.Models;
using POSTWebApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using POSTWebApi.ViewModels.Common;
using POSTWebApi.Repository.Interface;
using System.Threading.Tasks;
using POSTWebApi.Common.Interface;
using POSTWebApi.Common.Service;
using POSTWebApi.Services.Interfaces;
using System.Security.Claims;

namespace POSTWebApi.Repository
{
    public class UserRepository : IUserRepository
    {

        private IDbPOS _db;
        private string _secretKeyRefreshToken = "1zA12F24124FGasfas123ldsa";

        public UserRepository(IDbPOS db)
        {
            _db = db;
        }

        public Task<User> Create(User data)
        {
            return Task.Run(() =>
            {
                User user = _db.Users.Create();
                user.Email = data.Email;
                user.Password = data.Password;
                user.Name = data.Name;
                user.Phone = data.Phone;
                user.Gender = data.Gender;
                user.UserRoles = new List<UserRole>();
                _db.Users.Add(user);
                _db.SaveChanges();
                return user;
            });
        }

        public Task<User> Delete(Guid id)
        {
            return Task.Run(() => 
            {
                IEnumerable<ReceivedProduct> receivedProduct = _db.ReceivedProducts.Where(e => e.UserId == id);
                User user = _db.Users.Include(e => e.UserRoles.Select(a => a.Role))
                    .Where(e => e.Id == id).FirstOrDefault();
                if (user != null)
                {
                    if(receivedProduct.Count() != 0)
                    {
                        _db.ReceivedProducts.RemoveRange(receivedProduct);
                    }
                    _db.Users.Remove(user);
                    _db.SaveChanges();
                }
                return user;
            }); 
        }

        public Task<User> Get(Guid id)
        {
            return Task.Run(() => 
            {
                return _db.Users.Include(e => e.UserRoles.Select(a => a.Role))
                    .Where(e => e.Id == id)
                    .Where(e => e.DeletedAt == null).FirstOrDefault();
            });
        }

        public IEnumerable<User> GetAll(int skip, int limit)
        {
            return _db.Users.Include(e => e.UserRoles.Select(a => a.Role))
                    .Where(e => e.DeletedAt == null)
                    .OrderByDescending(e => e.UpdatedAt)
                    .Skip(skip)
                    .Take(limit);
        }

        public IEnumerable<User> GetAllDeleted(int skip, int limit)
        {
            return _db.Users.Include(e => e.UserRoles.Select(a => a.Role))
                    .Where(e => e.DeletedAt != null)
                    .OrderByDescending(e => e.DeletedAt)
                    .Skip(skip)
                    .Take(limit);
        }

        public Task<User> SoftDelete(Guid id)
        {
            return Task.Run(() =>
            {
                User user = _db.Users.Include(e => e.UserRoles.Select(a => a.Role))
                    .Where(e => e.Id == id).Where(e => e.DeletedAt == null).FirstOrDefault();
                if (user != null)
                {
                    user.DeletedAt = DateTime.UtcNow;
                    _db.SaveChanges();
                }
                return user;
            });
        }

        public Task<User> Update(Guid id, User data)
        {
            return Task.Run(() =>
            {
                User user = _db.Users.Include(e => e.UserRoles.Select(a => a.Role))
                  .Where(e => e.Id == id && e.DeletedAt == null).FirstOrDefault();
                if (user != null)
                {
                    user.Password = data.Password;
                    user.Name = data.Name;
                    user.Phone = data.Phone;
                    user.Gender = data.Gender;
                    user.UpdatedAt = DateTime.UtcNow;
                    _db.SaveChanges();
                }
                return user;
            });
        }

        public Task<TokenViewModel> Login(LoginViewModel data)
        {
            return Task.Run(() =>
            {
            
                User user = _db.Users.Include(e => e.UserRoles.Select(a => a.Role))
                .Where(e => e.Email == data.Email)
                .Where(e => e.Password == data.Password).FirstOrDefault();
                if(user == null)
                {
                    return null;
                }
                return new TokenViewModel(){
                    Token = _generateToken(user.Id, data.DeviceNumber),
                    RefreshToken = _generateRefreshToken(user.Id.ToString(), data.DeviceNumber)
                };
            });
        }

        public Task<TokenViewModel> RefreshToken(string refreshToken)
        {
            return Task.Run(() =>
            {
                IAuthService jwtService = new JWTService(_secretKeyRefreshToken);
                if (jwtService.IsTokenValid(refreshToken))
                {
                    List<Claim> claims = jwtService.GetTokenClaims(refreshToken).ToList();
                    string userId = claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.NameIdentifier)).Value;
                    string deviceNumber = claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.SerialNumber)).Value;
                    var id = new Guid(userId);
                    return new TokenViewModel()
                    {
                        RefreshToken = _generateRefreshToken(userId, deviceNumber),
                        Token = _generateToken(id, deviceNumber)
                    };
                }
                return null;
            });
        }

        public Task<string> Logout(string token, Guid id)
        {
            return Task.Run(() =>
            {
                User user = _db.Users.Where(e => e.Id == id)
                .Include(e => e.UserTokens).FirstOrDefault();

                if(user != null)
                {
                    UserToken userToken = user.UserTokens.Where(e => e.Token == token).FirstOrDefault();
                    if (userToken != null)
                    {
                        _db.UserTokens.Remove(userToken);
                        _db.SaveChanges();
                        return "Logout Success";
                    }
                }
                return null;
            });
        }

        public Task<string> LogoutAll(Guid userId)
        {
            return Task.Run(() =>
            {
                IEnumerable<UserToken> userTokens = _db.UserTokens.Where(e => e.UserId == userId).ToList();
                _db.UserTokens.RemoveRange(userTokens);
                _db.SaveChanges();
                return "Success";
            });
        }

        #region private method
        private string _generateRefreshToken(string id, string deviceNumber)
        {
            IAuthContainerModel jwtModel = JwtTokenService.GetAuthModel(id, deviceNumber);
            jwtModel.SecretKey = _secretKeyRefreshToken;
            jwtModel.ExpireMinutes = 13;
            IAuthService authService = new JWTService(jwtModel.SecretKey);
            return authService.GenerateToken(jwtModel);
        }

        private string _generateToken(Guid id, string deviceNumber)
        {
            string token = null;
            IAuthContainerModel jwtModel = JwtTokenService.GetAuthModel(id.ToString(), deviceNumber);
            IAuthService authService = new JWTService(jwtModel.SecretKey);
            token = authService.GenerateToken(jwtModel);
            UserToken userTokenIsValid = _db.UserTokens.Where(e => e.DeviceNumber == deviceNumber).FirstOrDefault();
            if (userTokenIsValid == null)
            {
                UserToken userToken = _db.UserTokens.Create();
                userToken.Token = token;
                userToken.UserId = id;
                userToken.DeviceNumber = deviceNumber;
                _db.UserTokens.Add(userToken);
                _db.SaveChanges();
            }
            else
            {
                userTokenIsValid.Token = token;
                userTokenIsValid.UpdatedAt = DateTime.UtcNow;
                _db.SaveChanges();
            }

            return token;
        }
        #endregion
    }
}