using POSTWebApi.Models;
using POSTWebApi.Repository;
using POSTWebApi.Services;
using POSTWebApi.Services.Attributes;
using POSTWebApi.Services.Interfaces;
using POSTWebApi.ViewModels.Entity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace POSTWebApi.Controllers
{
    public class SaleController : BaseApiController<Sale>
    {
        /*
        * @description : Constructor class for production and development side
        */
        public SaleController()
        {

        }

        /*
        * @description : Constructor class for testing or unit test
        */
        public SaleController(IDbPOS context)
        {
            saleRepository = new SaleRepository(context);
        }

        [HttpPost]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.CREATE_SALE)]
        public override async Task<IHttpActionResult> Create(Sale dataBody)
        {
            try
            {
                UserViewModel user = GetUserAuth();
                dataBody.UserId = user.Id;
                Sale sale = await saleRepository.Create(dataBody);
                SaleViewModel saleViewModel = new SaleViewModel(sale);
                saleViewModel.User = user;

                return new HttpJsonApiResult<SaleViewModel>(
                   saleViewModel, Request, HttpStatusCode.Created);
            }
            catch(Exception e)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.SOFT_DELETE_SALE)]
        public override async Task<IHttpActionResult> Delete(Guid id)
        {
            try
            {
                Sale sale = await saleRepository.SoftDelete(id);
                if (sale == null)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<SaleViewModel>(new SaleViewModel(sale), Request, HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [ActionName("Hard")]
        [JWTAuth(ConstantValue.FeatureValue.HARD_DELETE_SALE)]
        public async Task<IHttpActionResult> HardDelete(Guid id)
        {
            try
            {
                Sale sale = await saleRepository.Delete(id);
                if (sale == null)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<SaleViewModel>(new SaleViewModel(sale), Request, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPatch]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.EDIT_SALE)]
        public override async Task<IHttpActionResult> Edit(Guid id, Sale dataBody)
        {
            try
            {
                Sale sale = await saleRepository.Update(id, dataBody);
                if (sale == null)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<SaleViewModel>(
                    new SaleViewModel(sale),
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
        [JWTAuth(ConstantValue.FeatureValue.READ_SALE_LIST)]
        public override async Task<IHttpActionResult> Get([FromUri]PaginationQuery paginationQuery)
        {
            try
            {
                IEnumerable<Sale> sales = await Task.FromResult(saleRepository.GetAll(
                    paginationQuery.Skip, paginationQuery.Limit));

                return new HttpJsonApiResult<IEnumerable<SaleViewModel>>(
                    SaleViewModel.GetAll(sales),
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
        [JWTAuth(ConstantValue.FeatureValue.READ_DELETED_SALE_LIST)]
        public async Task<IHttpActionResult> GetDeletedProduct([FromUri] PaginationQuery paginationQuery)
        {
            try
            {
                IEnumerable<Sale> sales = await Task.FromResult(saleRepository.GetAllDeleted(
                        paginationQuery.Skip, paginationQuery.Limit));

                return new HttpJsonApiResult<IEnumerable<SaleViewModel>>(
                    SaleViewModel.GetAll(sales),
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
        [JWTAuth(ConstantValue.FeatureValue.READ_SALE)]
        public override async Task<IHttpActionResult> Get(Guid id)
        {
            try
            {
                Sale sale = await saleRepository.Get(id);
                if (sale == null)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<SaleDetailViewModel>(
                    new SaleDetailViewModel(sale), 
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
