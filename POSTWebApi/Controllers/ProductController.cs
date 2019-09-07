using POSTWebApi.Models;
using POSTWebApi.Repository;
using POSTWebApi.Services;
using POSTWebApi.Services.Attributes;
using POSTWebApi.Services.Interfaces;
using POSTWebApi.ViewModels.Entity;
using POSTWebApi.ViewModels.Insert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace POSTWebApi.Controllers
{
    public class ProductController : BaseApiController<Product>
    {
      /*
       * @description : Constructor class for production and development side
       */
        public ProductController()
        {
        }

        /*
         * @description : Constructor class for testing or unit test
         */
        public ProductController(IDbPOS context)
        {
            _db = context;
            productRepository = new ProductRepository(context);
            productCategoryRepository = new ProductCategoryRepository(context);
            priceRepository = new PriceRepository(context);
        }


        [HttpGet]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.READ_PRODUCT_LIST)]
        public override async Task<IHttpActionResult> Get([FromUri] PaginationQuery paginationQuery)
        {
            try
            {
                IEnumerable<Product> products = await Task.FromResult(productRepository
                    .GetAll(paginationQuery.Skip, paginationQuery.Limit));

                return new HttpJsonApiResult<IEnumerable<ProductViewModel>>(
                    ProductViewModel.GetAll(products), 
                    Request, 
                    HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [ActionName("Deleted")]
        [JWTAuth(ConstantValue.FeatureValue.READ_DELETED_PRODUCT_LIST)]
        public async Task<IHttpActionResult> GetDeletedProduct([FromUri] PaginationQuery paginationQuery)
        {
            try
            {
                IEnumerable<Product> products = await Task.FromResult(productRepository.GetAllDeleted(
                        paginationQuery.Skip, paginationQuery.Limit));

                return new HttpJsonApiResult<IEnumerable<ProductViewModel>>(
                    ProductViewModel.GetAll(products),
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
        [JWTAuth(ConstantValue.FeatureValue.READ_PRODUCT)]
        public override async Task<IHttpActionResult> Get(Guid id)
        {
            try
            {
                Product product = await productRepository.Get(id);
                if (product == null)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<ProductViewModel>(new ProductViewModel(product), Request, HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.CREATE_PRODUCT)]
        public override async Task<IHttpActionResult> Create(Product product)
        {
            try
            {
                Product newProduct = await productRepository.Create(product);
                return new HttpJsonApiResult<ProductViewModel>(
                    new ProductViewModel(newProduct), Request, HttpStatusCode.Created);
            }
            catch(Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPatch]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.EDIT_PRODUCT)]
        public override async Task<IHttpActionResult> Edit(Guid id, Product product)
        {
            try
            {
                Product replacedProduct = await productRepository.Update(id, product);
                if (replacedProduct == null)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<ProductViewModel>(
                    new ProductViewModel(replacedProduct), Request, HttpStatusCode.OK);
            }
            catch(Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [ActionName("Index")]
        [JWTAuth(ConstantValue.FeatureValue.SOFT_DELETE_PRODUCT)]
        public override async Task<IHttpActionResult> Delete(Guid id)
        {
            try
            {
                Product product = await productRepository.SoftDelete(id);
                if (product == null)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<ProductViewModel>(new ProductViewModel(product), Request, HttpStatusCode.OK);
            }
            catch(Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [ActionName("Hard")]
        [JWTAuth(ConstantValue.FeatureValue.HARD_DELETE_PRODUCT)]
        public async Task<IHttpActionResult> HardDeleteProduct(Guid id)
        {
            try
            {
                Product product = await productRepository.Delete(id);
                if (product == null)
                {
                    return new HttpJsonApiResult<string>("Not Found", Request, HttpStatusCode.NotFound);
                }
                return new HttpJsonApiResult<ProductViewModel>(new ProductViewModel(product), Request, HttpStatusCode.OK);
            }
            catch(Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [ActionName("Category")]
        [JWTAuth(ConstantValue.FeatureValue.CREATE_CATEGORY_PRODUCT)]
        public async Task<IHttpActionResult> AddCategory(Guid id, ProductCategoryInsertViewModel productCategories)
        {
            try
            {
                IEnumerable<ProductCategory> productCategory = await Task.FromResult(productCategoryRepository.Create(
                    productCategories.Categories.Select(e => new ProductCategory(){
                        ProductId = id,
                        CategoryId = e
                })));
                
                return new HttpJsonApiResult<IEnumerable<object>>(
                    productCategory.Select(e => new {
                        e.ProductId, e.CategoryId, e.CreatedAt, e.UpdatedAt
                }), Request, HttpStatusCode.Created);
            }
            catch (Exception e)
            {
                return new HttpJsonApiResult<string>(
                    "Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [ActionName("Category")]
        [JWTAuth(ConstantValue.FeatureValue.DELETE_CATEGORY_PRODUCT)]
        public async Task<IHttpActionResult> DeleteCategory(Guid id, ProductCategoryInsertViewModel productCategories)
        {
            try
            {
                IEnumerable<ProductCategory> productCategory = await Task.FromResult(productCategoryRepository
                    .Delete(productCategories.Categories, id));

                return new HttpJsonApiResult<IEnumerable<object>>(
                    productCategory.Select(e => new {
                        e.ProductId, e.CategoryId, e.CreatedAt, e.UpdatedAt
                }), Request, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return new HttpJsonApiResult<string>(
                    "Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [ActionName("SalePrice")]
        [JWTAuth(ConstantValue.FeatureValue.CREATE_SALE_PRICE_PRODUCT)]
        public async Task<IHttpActionResult> CreateSalePrice(PriceInsertViewModel price)
        {
            try
            {
                Price newPrice = await priceRepository.Create(new Price()
                {
                    Type = PriceTypeEnum.Sale,
                    Value = price.Value,
                    ProductId = price.ProductId
                });
                return new HttpJsonApiResult<PriceViewModel>(
                    new PriceViewModel(newPrice), Request, HttpStatusCode.Created);
            }
            catch (Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [ActionName("SalePrice")]
        [JWTAuth(ConstantValue.FeatureValue.READ_SALE_PRICES_PRODUCT)]
        public async Task<IHttpActionResult> ReadSalePrices([FromUri] PaginationQuery paginationQuery)
        {
            try
            {
                IEnumerable<Price> prices = await Task.FromResult(priceRepository.GetAll(
                        PriceTypeEnum.Sale, paginationQuery.Skip,
                        paginationQuery.Limit));

                return new HttpJsonApiResult<IEnumerable<PriceViewModel>>(
                    PriceViewModel.GetAll(prices),
                    Request,
                    HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [ActionName("PurchasePrice")]
        [JWTAuth(ConstantValue.FeatureValue.CREATE_PURCHASE_PRICE_PRODUCT)]
        public async Task<IHttpActionResult> CreatePurchasePrice(PriceInsertViewModel price)
        {
            try
            {
                Price newPrice = await priceRepository.Create(new Price()
                {
                    Type = PriceTypeEnum.Purchase,
                    Value = price.Value,
                    ProductId = price.ProductId
                });

                return new HttpJsonApiResult<PriceViewModel>(
                    new PriceViewModel(newPrice), Request, HttpStatusCode.Created);
            }
            catch (Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [ActionName("PurchasePrice")]
        [JWTAuth(ConstantValue.FeatureValue.READ_PURCHASE_PRICES_PRODUCT)]
        public async Task<IHttpActionResult> ReadPurchasePrices([FromUri] PaginationQuery paginationQuery)
        {
            try
            {
                IEnumerable<Price> prices = await Task.FromResult(priceRepository.GetAll(
                        PriceTypeEnum.Purchase, paginationQuery.Skip,
                        paginationQuery.Limit));

                return new HttpJsonApiResult<IEnumerable<PriceViewModel>>(
                    PriceViewModel.GetAll(prices),
                    Request,
                    HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return new HttpJsonApiResult<string>("Internal Server Error", Request, HttpStatusCode.InternalServerError);
            }
        }
    }
}
