using System.Security.Claims;
using POSTWebApi.Common.Interface;
using POSTWebApi.Common.Service;

namespace POSTWebApi.Services
{
    public class JwtTokenService
    {
        public static IAuthContainerModel GetAuthModel(string id = "", string deviceNumber = "")
        {
            return new JWTContainerModel()
            {
                Claims = new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, id),
                    new Claim(ClaimTypes.SerialNumber, deviceNumber)
                }
            };
        }
    }
}