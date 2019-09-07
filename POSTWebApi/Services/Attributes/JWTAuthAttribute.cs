using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using POSTWebApi.Common.Service;
using POSTWebApi.Common.Interface;
using System.Security.Claims;
using POSTWebApi.Models;
using POSTWebApi.ViewModels.Entity;

namespace POSTWebApi.Services.Attributes
{
    public class JWTAuthAttribute : AuthorizeAttribute
    {
        private string _featureName;
        private JWTService _jwtService;
        private IObjectService _objectService;
        private IAuthContainerModel _authModel;
        protected DbPOS db;


        public JWTAuthAttribute(string featureName)
        {
            _featureName = featureName;
            _objectService = new ObjectService<UserViewModel>();
            _authModel = JwtTokenService.GetAuthModel();
            _jwtService = new JWTService(_authModel.SecretKey);
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                db = new DbManagement().db;
                if (actionContext.Request.Headers.Authorization == null)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                        new HttpJsonApiResult<string>("Unauthorized", actionContext.Request, HttpStatusCode.Unauthorized));
                }
                else
                {
                    string authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                    if (_jwtService.IsTokenValid(authenticationToken))
                    {
                        List<Claim> claims = _jwtService.GetTokenClaims(authenticationToken).ToList();
                        string userId = claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.NameIdentifier)).Value;
                        User user = db.Users.Where(e => e.Id == new Guid(userId))
                            .Include(e => e.UserRoles.Select(a => a.Role))
                            .Include(e => e.UserTokens).FirstOrDefault();
                        ICollection <UserRole> userRoles = user.UserRoles;
                        UserViewModel userAuthObject = new UserViewModel(user);
                        if (user.UserTokens.Where(e => e.Token == authenticationToken).Count() != 0)
                        {
                            if (userRoles.Where(e => e.Role.Name == "superuser").FirstOrDefault() != null)
                            {
                                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(_objectService.SerializerObject(userAuthObject)), null);
                            }
                            else
                            {
                                UserRole thisRoleCanAccess = userRoles.Where(e => e.Role.RoleFeatures.Where(
                                    x => x.Feature.Name == _featureName).FirstOrDefault() != null).FirstOrDefault();
                                if (thisRoleCanAccess != null)
                                {
                                    db.Configuration.ProxyCreationEnabled = false;
                                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(_objectService.SerializerObject(userAuthObject)), null);
                                }
                                else
                                {
                                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                                        new HttpJsonApiResult<string>("Unauthorized", actionContext.Request, HttpStatusCode.Unauthorized));
                                }
                            }
                        }
                        else
                        {
                            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                                new HttpJsonApiResult<string>("Unauthorized", actionContext.Request, HttpStatusCode.Unauthorized));
                        }
                    }
                    else
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                            new HttpJsonApiResult<string>("Unauthorized", actionContext.Request, HttpStatusCode.Unauthorized));
                    }
                }
            }
            catch(Exception)
            {
                actionContext.Response =  actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, 
                    new HttpJsonApiResult<string>("Unauthorized", actionContext.Request, HttpStatusCode.Unauthorized));
            }
        }
    }
}