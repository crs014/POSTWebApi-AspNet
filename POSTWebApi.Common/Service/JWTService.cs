using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security.Jwt;
using POSTWebApi.Common.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace POSTWebApi.Common.Service
{
    public class JWTService : IAuthService
    {
        public string SecretKey { get; set; }
        private byte[] _signKey;
        private JwtSecurityTokenHandler _handler;
        private SymmetricSecurityKey _securityKey;
        private SigningCredentials _credentials;
        private SecurityToken _securityToken;
        private TokenValidationParameters _tokenValidationParameters;
        public JWTService(string secretKey)
        {
            SecretKey = secretKey;
            _handler = new JwtSecurityTokenHandler();
            _signKey = Encoding.UTF8.GetBytes(SecretKey);
            _tokenValidationParameters = new TokenValidationParameters()
            {
                IssuerSigningKeys = new SymmetricKeyIssuerSecurityKeyProvider("issuer", _signKey).SecurityKeys,
                ValidateIssuer = false,
                ValidateAudience = false
                //ValidateLifetime = false,
            };
        }

        public bool IsTokenValid(string token)
        {
            
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("given token null or empty.");
            }

            try
            { 
                ClaimsPrincipal tokenValid = _handler.ValidateToken(token, _tokenValidationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public string GenerateToken(IAuthContainerModel model)
        {
            if(model == null || model.Claims == null || model.Claims.Length == 0)
            {
                throw new ArgumentException("Arguments to create token are not valid.");
            }
            _securityKey = new SymmetricSecurityKey(_signKey);
            _credentials = new SigningCredentials(_securityKey, model.SecurityAlgorithm);
            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(model.Claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(model.ExpireMinutes)),
                SigningCredentials = _credentials
            };
            _securityToken = _handler.CreateToken(securityTokenDescriptor);
            return _handler.WriteToken(_securityToken);
        }

        public IEnumerable<Claim> GetTokenClaims(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("given token is null or empty");
            }

            try
            {
                ClaimsPrincipal tokenValid = _handler.ValidateToken(token, _tokenValidationParameters, out SecurityToken validatedToken);
                return tokenValid.Claims;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
