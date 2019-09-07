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
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace POSTWebApi.Controllers
{
    public class ReceivedProductController : BaseApiController<ReceivedProduct>
    {

        /*
         * @description : Property for put data user login for testing and production
         */
        protected UserViewModel userDataLogin;

        /*
        * @description : Constructor class for production and development side
        */
        public ReceivedProductController()
        {

        }

        /*
        * @description : Constructor class for testing or unit test
        */
        public ReceivedProductController(IDbPOS context, User user)
        {
            userDataLogin = new UserViewModel(user);
            receivedProductRepository = new ReceivedProductRepository(context);
        }

        [HttpPost]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.CREATE_RECEIPT_OF_PRODUCT)]
        public override async Task<IHttpActionResult> Create(ReceivedProduct dataBody)
        {
            try
            {
                if(GetUserAuth() != null)
                {
                    userDataLogin = GetUserAuth();
                }
                dataBody.UserId = userDataLogin.Id;
                ReceivedProduct receivedProduct = await receivedProductRepository.Create(dataBody);
                ReceivedProductViewModel receivedProductViewModel = new ReceivedProductViewModel(receivedProduct);
                receivedProductViewModel.User = userDataLogin;

                return new HttpJsonApiResult<ReceivedProductViewModel>(
                    receivedProductViewModel, Request, HttpStatusCode.Created);
            }
            catch(Exception e)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.DELETE_RECEIPT_OF_PRODUCT)]
        public override async Task<IHttpActionResult> Delete(Guid id)
        {
            try
            {
                ReceivedProduct receivedProduct = await receivedProductRepository.Delete(id);
                if(receivedProduct == null)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<ReceivedProductViewModel>(new ReceivedProductViewModel(receivedProduct), Request, HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPatch]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.EDIT_RECEIPT_OF_PRODUCT)]
        public override async Task<IHttpActionResult> Edit(Guid id, ReceivedProduct dataBody)
        {
            try
            {
                ReceivedProduct receivedProduct = await receivedProductRepository.Update(id, dataBody);
                if (receivedProduct == null)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<ReceivedProductViewModel>(
                    new ReceivedProductViewModel(receivedProduct),
                    Request,
                    HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.READ_RECEIPT_OF_PRODUCT_LIST)]
        public override async Task<IHttpActionResult> Get([FromUri] PaginationQuery paginationQuery)
        {
            try
            {
                IEnumerable<ReceivedProduct> receivedProducts = await Task.FromResult(receivedProductRepository.GetAll(
                        paginationQuery.Skip, paginationQuery.Limit));

                return new HttpJsonApiResult<IEnumerable<ReceivedProductViewModel>>(
                       ReceivedProductViewModel.GetAll(receivedProducts),
                       Request,
                       HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.READ_RECEIPT_OF_PRODUCT)]
        public override async Task<IHttpActionResult> Get(Guid id)
        {
            try
            {
                IEnumerable<ReceivedProduct> receivedProducts = await Task.FromResult(receivedProductRepository.Get(id));

                if (receivedProducts == null || receivedProducts.Count() == 0)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }

                return new HttpJsonApiResult<IEnumerable<ReceivedProductViewModel>>(
                     ReceivedProductViewModel.GetAll(receivedProducts),
                     Request,
                     HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }
    }
}
