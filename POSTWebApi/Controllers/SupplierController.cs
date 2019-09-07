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
    public class SupplierController : BaseApiController<Supplier>
    {

        /*
         * @description : Constructor class for production and development side
         */
        public SupplierController()
        {
        }

        /*
         * @description : Constructor class for testing or unit test
         */
        public SupplierController(IDbPOS context)
        {
            _db = context;
            supplierRepository = new SupplierRepository(context);
        }

        [HttpGet]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.READ_SUPPLIER_LIST)]
        public override async Task<IHttpActionResult> Get([FromUri] PaginationQuery paginationQuery)
        {
            try
            {
                IEnumerable<Supplier> suppliers = await Task.FromResult(supplierRepository.GetAll(
                        paginationQuery.Skip, paginationQuery.Limit));

                return new HttpJsonApiResult<IEnumerable<SupplierViewModel>>(
                    SupplierViewModel.GetAll(suppliers),
                    Request,
                    HttpStatusCode.OK);
            }
            catch(Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [ActionName("Deleted")]
        [JWTAuth(ConstantValue.FeatureValue.READ_DELETED_SUPPLIER_LIST)]
        public async Task<IHttpActionResult> GetDeletedSupplier([FromUri] PaginationQuery paginationQuery)
        {
            try
            {
                IEnumerable<Supplier> suppliers = await Task.FromResult(supplierRepository.GetAllDeleted(
                        paginationQuery.Skip, paginationQuery.Limit));

                return new HttpJsonApiResult<IEnumerable<SupplierViewModel>>(
                    SupplierViewModel.GetAll(suppliers),
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
        [JWTAuth(ConstantValue.FeatureValue.READ_SUPPLIER)]
        public override async Task<IHttpActionResult> Get(Guid id)
        {
            try
            {
                Supplier supplier = await supplierRepository.Get(id);
                if (supplier == null)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<SupplierViewModel>(new SupplierViewModel(supplier), Request, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            } 
        }

        [HttpPost]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.CREATE_SUPPLIER)]
        public override async Task<IHttpActionResult> Create(Supplier supplier)
        {
            try
            {
                Supplier newSupplier = await supplierRepository.Create(supplier);
                return new HttpJsonApiResult<SupplierViewModel>(new SupplierViewModel(newSupplier), Request, HttpStatusCode.Created);
            }
            catch (Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPatch]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.EDIT_SUPPLIER)]
        public override async Task<IHttpActionResult> Edit(Guid id, Supplier supplier)
        {
            try
            {
                Supplier replacedSupplier = await supplierRepository.Update(id, supplier);
                if (replacedSupplier == null)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<SupplierViewModel>(new SupplierViewModel(replacedSupplier), Request, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.SOFT_DELETE_SUPPLIER)]
        public override async Task<IHttpActionResult> Delete(Guid id)
        {
            try
            {
                Supplier supplier = await supplierRepository.SoftDelete(id);
                if (supplier == null)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<SupplierViewModel>(new SupplierViewModel(supplier), Request, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [ActionName("Hard")]
        [JWTAuth(ConstantValue.FeatureValue.HARD_DELETE_SUPPLIER)]
        public async Task<IHttpActionResult> HardDeleteSupplier(Guid id)
        {
            try
            {
                Supplier supplier = await supplierRepository.Delete(id);
                if (supplier == null)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<SupplierViewModel>(new SupplierViewModel(supplier), Request, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }
    }
}
