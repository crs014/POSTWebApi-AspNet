using POSTWebApi.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using POSTWebApi.Services;
using POSTWebApi.Services.Attributes;
using POSTWebApi.ViewModels.Common;
using POSTWebApi.ViewModels.Entity;
using System.Threading.Tasks;
using System.Web.Http.Results;
using POSTWebApi.Services.Interfaces;
using POSTWebApi.Repository;

namespace POSTWebApi.Controllers
{
    public class UserController : BaseApiController<User>
    {

        /*
         * @description : Property for put data user login for testing and production
         */
        protected UserViewModel userDataLogin;

        /*
         * @description : Constructor class for production and development side
         */
        public UserController()
        {
        }

        /*
         * @description : Constructor class for testing or unit test
         */
        public UserController(IDbPOS context, User user)
        {
            _db = context;
            userRepository = new UserRepository(_db);
            userDataLogin = new UserViewModel(user);
        }

        [HttpGet]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.READ_USER_LIST)]
        public override async Task<IHttpActionResult> Get([FromUri] PaginationQuery paginationQuery)
        {
            try
            {
                IEnumerable<User> users = await Task.FromResult(userRepository
                    .GetAll(paginationQuery.Skip, paginationQuery.Limit));

                return new HttpJsonApiResult<IEnumerable<UserViewModel>>(
                    UserViewModel.GetAll(users),
                    Request,
                    HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.READ_USER)]
        public override async Task<IHttpActionResult> Get(Guid id)
        {
            try
            {
                User user = await userRepository.Get(id);
                if (user == null)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<UserViewModel>(new UserViewModel(user), Request, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [ActionName("Deleted")]
        [JWTAuth(ConstantValue.FeatureValue.READ_DELETED_USER_LIST)]
        public async Task<IHttpActionResult> GetDeletedUser([FromUri] PaginationQuery paginationQuery)
        {
            try
            {
                IEnumerable<User> users = await Task.FromResult(userRepository
                    .GetAllDeleted(paginationQuery.Skip, paginationQuery.Limit));

                return new HttpJsonApiResult<IEnumerable<UserViewModel>>(
                    UserViewModel.GetAll(users),
                    Request, 
                    HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [ActionName("Login")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Login(LoginViewModel dataBody)
        {
            try
            {
                TokenViewModel token = await userRepository.Login(dataBody);
                if(token == null)
                {
                    return new HttpJsonApiResult<string>("User Are Not Registered", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<TokenViewModel>(token, Request, HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [ActionName("RefreshToken")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> RefreshToken(WebTokenViewModel dataBody)
        {
            try
            {
                TokenViewModel token = await userRepository.RefreshToken(dataBody.Token);
                if (token == null)
                {
                    return new HttpJsonApiResult<string>("Token are not valid", Request, HttpStatusCode.BadRequest);
                }
                return new HttpJsonApiResult<TokenViewModel>(token, Request, HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [ActionName("Logout")]
        [JWTAuth(ConstantValue.FeatureValue.LOGOUT)]
        public async Task<IHttpActionResult> Logout(WebTokenViewModel dataBody)
        {
            try
            {
                if(GetUserAuth() != null)
                {
                    userDataLogin = GetUserAuth();
                }

                string isSuccess = await userRepository.Logout(dataBody.Token,userDataLogin.Id);
                if (isSuccess == null)
                {
                    return new HttpJsonApiResult<string>("Token are not valid", Request, HttpStatusCode.BadRequest);
                }
                return new HttpJsonApiResult<string>(isSuccess, Request, HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [ActionName("LogoutAll")]
        [JWTAuth(ConstantValue.FeatureValue.LOGOUT_ALL)]
        public async Task<IHttpActionResult> LogoutAll()
        {
            try
            {
                if (GetUserAuth() != null)
                {
                    userDataLogin = GetUserAuth();
                }
                string logout = await userRepository.LogoutAll(userDataLogin.Id);
                return new HttpJsonApiResult<string>(logout, Request, HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [ActionName("Self")]
        [JWTAuth(ConstantValue.FeatureValue.READ_SELF_USER)]
        public async Task<IHttpActionResult> ReadSelf()
        {
            try
            {
                if(GetUserAuth()!= null)
                {
                    userDataLogin = GetUserAuth();
                }

                UserViewModel user = await Task.FromResult(GetUserAuth()); 
                return new HttpJsonApiResult<UserViewModel>(user, Request, HttpStatusCode.OK); 
            }
            catch(Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.CREATE_USER)]
        public override async Task<IHttpActionResult> Create(User dataBody)
        {
            try
            {
                User user = await userRepository.Create(dataBody);
                return new HttpJsonApiResult<UserViewModel>(new UserViewModel(user), Request, HttpStatusCode.Created);
            }
            catch (Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        //plan pakai viewModel
        [HttpPatch]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.EDIT_USER)]
        public override async Task<IHttpActionResult> Edit(Guid id, User dataBody)
        {
            try
            {
                User user = await userRepository.Update(id, dataBody);
                if (user == null)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<UserViewModel>(new UserViewModel(user), Request, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.SOFT_DELETE_USER)]
        public override async Task<IHttpActionResult> Delete(Guid id)
        {
            try
            {
                User user = await userRepository.SoftDelete(id);
                if (user == null)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<UserViewModel>(new UserViewModel(user), Request, HttpStatusCode.OK);
            }
            catch(Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [ActionName("Hard")]
        [JWTAuth(ConstantValue.FeatureValue.HARD_DELETE_PRODUCT)]
        public async Task<IHttpActionResult> HardDeleteUser(Guid id)
        {
            try
            {
                User user = await userRepository.Delete(id); 
                if (user == null)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<UserViewModel>(new UserViewModel(user), Request, HttpStatusCode.OK);
            }
            catch(Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }
    }
}
