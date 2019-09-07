using POSTWebApi.Models;
using POSTWebApi.Repository;
using POSTWebApi.Services;
using POSTWebApi.Services.Attributes;
using POSTWebApi.Services.Interfaces;
using POSTWebApi.ViewModels.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace POSTWebApi.Controllers
{
    public class RoleController : BaseApiController<Role>
    {

        /*
         * @description : Constructor class for production and development side
         */
        public RoleController()
        {
        }

        /*
         * @description : Constructor class for testing or unit test
         */
        public RoleController(IDbPOS context)
        {
            _db = context;
            roleRepository = new RoleRepository(context);
        }

        [HttpGet]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.READ_ROLE_LIST)]
        public override async Task<IHttpActionResult> Get([FromUri] PaginationQuery paginationQuery)
        {
            try
            {
                IEnumerable<Role> roles = await Task.FromResult(roleRepository
                    .GetAll(paginationQuery.Skip, paginationQuery.Limit));

                return new HttpJsonApiResult<IEnumerable<RoleViewModel>>(
                    RoleViewModel.GetAll(roles),
                    Request,
                    HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [ActionName("Deleted")]
        [JWTAuth(ConstantValue.FeatureValue.READ_DELETED_ROLE_LIST)]
        public async Task<IHttpActionResult> GetDeletedRole([FromUri] PaginationQuery paginationQuery)
        {
            try
            {
                IEnumerable<Role> roles = await Task.FromResult(roleRepository
                    .GetAllDeleted(paginationQuery.Skip, paginationQuery.Limit));

                return new HttpJsonApiResult<IEnumerable<RoleViewModel>>(
                    RoleViewModel.GetAll(roles),
                    Request,
                    HttpStatusCode.OK);
            }
            catch(Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.READ_ROLE)]
        public override async Task<IHttpActionResult> Get(Guid id)
        {
            try
            {
                Role role = await roleRepository.Get(id);
                if (role == null)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<RoleViewModel>(new RoleViewModel(role), Request, HttpStatusCode.OK);
            }
            catch(Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.CREATE_ROLE)]
        public override async Task<IHttpActionResult> Create(Role dataBody)
        {
            try
            {
                Role role = await roleRepository.Create(dataBody);
                return new HttpJsonApiResult<RoleViewModel>(new RoleViewModel(role), Request, HttpStatusCode.Created);
            }
            catch(Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPatch]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.EDIT_ROLE)]
        public override async Task<IHttpActionResult> Edit(Guid id, Role dataBody)
        {
            try
            {
                Role role = await roleRepository.Update(id, dataBody);
                if (role == null)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<RoleViewModel>(new RoleViewModel(role), Request, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.SOFT_DELETE_ROLE)]
        public override async Task<IHttpActionResult> Delete(Guid id)
        {
            try
            {
                Role role = await roleRepository.SoftDelete(id);
                if (role == null)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<RoleViewModel>(new RoleViewModel(role), Request, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [ActionName("Hard")]
        [JWTAuth(ConstantValue.FeatureValue.HARD_DELETE_ROLE)]
        public async Task<IHttpActionResult> HardDeleteRole(Guid id)
        {
            try
            {
                Role role = await roleRepository.Delete(id);
                if (role == null)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<RoleViewModel>(new RoleViewModel(role), Request, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

    }
}