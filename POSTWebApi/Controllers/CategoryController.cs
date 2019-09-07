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
    public class CategoryController : BaseApiController<Category>
    {

        /*
         * @description : Constructor class for production and development side
         */
        public CategoryController()
        {
        }

        /*
         * @description : Constructor class for testing or unit test
         */
        public CategoryController(IDbPOS context)
        {
            _db = context;
            categoryRepository = new CategoryRepository(context);
        }

        [HttpGet]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.READ_CATEGORY_LIST)]
        public override async Task<IHttpActionResult> Get([FromUri] PaginationQuery paginationQuery)
        {
            try
            {
                IEnumerable<Category> categories = await Task.FromResult(categoryRepository
                    .GetAll(paginationQuery.Skip, paginationQuery.Limit));

                return new HttpJsonApiResult<IEnumerable<CategoryViewModel>>(
                    CategoryViewModel.GetAll(categories),
                    Request,
                    HttpStatusCode.OK
                );
            }
            catch (Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [ActionName("Deleted")]
        [JWTAuth(ConstantValue.FeatureValue.READ_DELETED_CATEGORY_LIST)]
        public async Task<IHttpActionResult> GetDeletedCategory([FromUri] PaginationQuery paginationQuery)
        {
            try
            {
                IEnumerable<Category> categories = await Task.FromResult(categoryRepository
                    .GetAllDeleted(paginationQuery.Skip, paginationQuery.Limit));

                return new HttpJsonApiResult<IEnumerable<CategoryViewModel>>(
                    CategoryViewModel.GetAll(categories),
                    Request,
                    HttpStatusCode.OK
                );
            }
            catch (Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.READ_CATEGORY)]
        public override async Task<IHttpActionResult> Get(Guid id)
        {
            try
            {
                Category category = await categoryRepository.Get(id);
                if (category == null)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<CategoryViewModel>(new CategoryViewModel(category), Request, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.CREATE_CATEGORY)]
        public override async Task<IHttpActionResult> Create(Category category)
        {
            try
            {
                Category newCategory = await categoryRepository.Create(category);
                return new HttpJsonApiResult<CategoryViewModel>(new CategoryViewModel(newCategory), Request, HttpStatusCode.Created);
            }
            catch (Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPatch]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.EDIT_CATEGORY)]
        public override async Task<IHttpActionResult> Edit(Guid id, Category category)
        {
            try
            {
                Category replacedCategory = await categoryRepository.Update(id, category);
                if (replacedCategory == null)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<CategoryViewModel>(new CategoryViewModel(replacedCategory), Request, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.SOFT_DELETE_CATEGORY)]
        public override async Task<IHttpActionResult> Delete(Guid id)
        {
            try
            {
                Category category = await categoryRepository.SoftDelete(id);
                if (category == null)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<CategoryViewModel>(new CategoryViewModel(category), Request, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [ActionName("Hard")]
        [JWTAuth(ConstantValue.FeatureValue.HARD_DELETE_CATEGORY)]
        public async Task<IHttpActionResult> HardDeleteProduct(Guid id)
        {
            try
            {
                Category category = await categoryRepository.Delete(id);
                if (category == null)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<CategoryViewModel>(new CategoryViewModel(category), Request, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }
    }
}
