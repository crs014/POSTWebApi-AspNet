using Microsoft.IdentityModel.Tokens;
using POSTWebApi.Common.Interface;
using System.Security.Claims;

namespace POSTWebApi.Common.Service
{
    public class JWTContainerModel : IAuthContainerModel
    {
        public int ExpireMinutes { get; set; } = 10;
        public string SecretKey { get; set; } = "TaU77T1iX123XJPxda4rSDK";
        public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;
        public Claim[] Claims { get; set; }
    }
}
