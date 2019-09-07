using POSTWebApi.Models;
using POSTWebApi.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using POSTWebApi.ViewModels.Entity;
using POSTWebApi.Services.Attributes;
using System.Threading.Tasks;
using POSTWebApi.Repository;
using POSTWebApi.Services.Interfaces;

namespace POSTWebApi.Controllers
{
    public class PurchaseController : BaseApiController<Purchase>
    {
        /*
        * @description : Constructor class for production and development side
        */
        public PurchaseController()
        {

        }

        /*
        * @description : Constructor class for testing or unit test
        */
        public PurchaseController(IDbPOS context)
        {
            purchaseRepository = new PurchaseRepository(context);
        }

        [HttpPost]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.CREATE_PURCHASE)]
        public override async Task<IHttpActionResult> Create(Purchase dataBody)
        {
            try
            {
                UserViewModel user = GetUserAuth();
                dataBody.UserId = user.Id;
                Purchase purchase = await purchaseRepository.Create(dataBody);
                PurchaseViewModel purchaseViewModel = new PurchaseViewModel(purchase);
                purchaseViewModel.User = user;

                return new HttpJsonApiResult<PurchaseViewModel>(
                  purchaseViewModel, Request, HttpStatusCode.Created);
            }
            catch(Exception e)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.SOFT_DELETE_PURCHASE)]
        public override async Task<IHttpActionResult> Delete(Guid id)
        {
            try
            {
                Purchase purchase = await purchaseRepository.SoftDelete(id);
                if(purchase == null)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<PurchaseViewModel>(new PurchaseViewModel(purchase), Request, HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [ActionName("Hard")]
        [JWTAuth(ConstantValue.FeatureValue.HARD_DELETE_PURCHASE)]
        public async Task<IHttpActionResult> HardDeleteProduct(Guid id)
        {
            try
            {
                Purchase purchase = await purchaseRepository.Delete(id);
                if (purchase == null)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<PurchaseViewModel>(new PurchaseViewModel(purchase), Request, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPatch]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.EDIT_PURCHASE)]
        public override async Task<IHttpActionResult> Edit(Guid id, Purchase dataBody)
        {
            try
            {
                Purchase purchase = await purchaseRepository.Update(id, dataBody);
                if(purchase == null)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<PurchaseViewModel>(
                    new PurchaseViewModel(purchase),
                    Request,
                    HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.READ_PURCHASE_LIST)]
        public override async Task<IHttpActionResult> Get([FromUri] PaginationQuery paginationQuery)
        {
            try
            {
                IEnumerable<Purchase> purchases = await Task.FromResult(purchaseRepository.GetAll(
                    paginationQuery.Skip, paginationQuery.Limit));

                return new HttpJsonApiResult<IEnumerable<PurchaseViewModel>>(
                    PurchaseViewModel.GetAll(purchases),
                    Request,
                    HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [ActionName("Deleted")]
        [JWTAuth(ConstantValue.FeatureValue.READ_DELETED_PURCHASE_LIST)]
        public async Task<IHttpActionResult> GetDeletedProduct([FromUri] PaginationQuery paginationQuery)
        {
            try
            {
                IEnumerable<Purchase> purchases = await Task.FromResult(purchaseRepository.GetAllDeleted(
                        paginationQuery.Skip, paginationQuery.Limit));

                return new HttpJsonApiResult<IEnumerable<PurchaseViewModel>>(
                    PurchaseViewModel.GetAll(purchases),
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
        [JWTAuth(ConstantValue.FeatureValue.READ_PURCHASE)]
        public override async Task<IHttpActionResult> Get(Guid id)
        {
            try
            {
                Purchase purchase = await purchaseRepository.Get(id);
                if(purchase == null)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<PurchaseDetailViewModel>(new PurchaseDetailViewModel(purchase), Request, HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }
    }
}
