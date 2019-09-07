using Microsoft.VisualStudio.TestTools.UnitTesting;
using POSTWebApi.Controllers;
using POSTWebApi.Models;
using POSTWebApi.Services;
using POSTWebApi.Services.Interfaces;
using POSTWebApi.Testing.Services;
using POSTWebApi.ViewModels.Entity;
using POSTWebApi.ViewModels.Insert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace POSTWebApi.Testing
{
    [TestClass]
    public class TestProductController
    {
        private IDbPOS storeAppContext;
        private ProductController productController;

        public TestProductController()
        {
            #region load dummy data
            storeAppContext = new TestDbPOS();
            storeAppContext.Products.Add(new Product()
            {
                Id = new Guid("223913df-203a-41e2-83a9-94343ee5d434"),
                Name = "Katana",
                Description = "its sharp",
                ProductCategories = new List<ProductCategory>(),
                Prices = new List<Price>()
            });
            storeAppContext.Products.Add(new Product()
            {
                Id = new Guid("5579b911-2392-43f8-87cf-83e1a7bbed6f"),
                Name = "Hard Mail",
                Description = "its hard",
                ProductCategories = new List<ProductCategory>(),
                Prices = new List<Price>()
            });
            #endregion

            #region load controller
            productController = new ProductController(storeAppContext);
            #endregion
        }

        [TestMethod]
        public async Task ReadProductList_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await productController.Get(new BaseApiController<Product>.PaginationQuery()
            {
                Limit = 2,
                Skip = 0
            });
            HttpJsonApiResult<IEnumerable<ProductViewModel>> contentResult = result as HttpJsonApiResult<IEnumerable<ProductViewModel>>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task ReadProduct_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await productController.Get(new Guid("223913df-203a-41e2-83a9-94343ee5d434"));
            HttpJsonApiResult<ProductViewModel> contentResult = result as HttpJsonApiResult<ProductViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task ReadProduct_NotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await productController.Get(Guid.NewGuid());
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task CreateProduct_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await productController.Create(new Product()
            {
                Name = "Dynamite",
                Description = "KABOOM" +
                ""
            });
            HttpJsonApiResult<ProductViewModel> contentResult = result as HttpJsonApiResult<ProductViewModel>;
            Assert.AreEqual(HttpStatusCode.Created, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task EditProduct_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await productController.Edit(new Guid("223913df-203a-41e2-83a9-94343ee5d434"), new Product() {
                Name = "Zangetsu",
                Description = "ichigo kurosaki zanpakutou"
            });
            HttpJsonApiResult<ProductViewModel> contentResult = result as HttpJsonApiResult<ProductViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task EditProduct_NotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await productController.Edit(Guid.NewGuid(), new Product()
            {
                Name = "Zangetsu",
                Description = "ichigo kurosaki zanpakutou"
            });
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task SoftDeleteProduct_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await productController.Delete(new Guid("223913df-203a-41e2-83a9-94343ee5d434"));
            HttpJsonApiResult<ProductViewModel> contentResult = result as HttpJsonApiResult<ProductViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task SoftDeleteProduct_NotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await productController.Delete(Guid.NewGuid());
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task HardDeleteProduct_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await productController.HardDeleteProduct(new Guid("5579b911-2392-43f8-87cf-83e1a7bbed6f"));
            HttpJsonApiResult<ProductViewModel> contentResult = result as HttpJsonApiResult<ProductViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task HardDeleteProduct_NotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await productController.HardDeleteProduct(Guid.NewGuid());
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task CreateProductCategory_Success_ReturnStatusCodeCreated()
        {
            IHttpActionResult result = await productController.AddCategory(
                new Guid("223913df-203a-41e2-83a9-94343ee5d434"), new ProductCategoryInsertViewModel() {
                Categories = new List<Guid>()
                {
                    new Guid("223913df-203a-41e2-83a9-94343ee5d434"),
                    new Guid("5579b911-2392-43f8-87cf-83e1a7bbed6f")
                }
            });
            HttpJsonApiResult<IEnumerable<object>> contentResult = result as HttpJsonApiResult<IEnumerable<object>>;
            Assert.AreEqual(HttpStatusCode.Created, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task DeleteProductCategory_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await productController.DeleteCategory(
                new Guid("223913df-203a-41e2-83a9-94343ee5d434"), new ProductCategoryInsertViewModel()
                {
                    Categories = new List<Guid>()
                {
                    new Guid("223913df-203a-41e2-83a9-94343ee5d434"),
                    new Guid("5579b911-2392-43f8-87cf-83e1a7bbed6f")
                }
                });
            HttpJsonApiResult<IEnumerable<object>> contentResult = result as HttpJsonApiResult<IEnumerable<object>>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task CreateSalePrice_Success_ReturnStatusCodeCreated()
        {
            IHttpActionResult result = await productController.CreateSalePrice(new PriceInsertViewModel()
            {
                ProductId = new Guid("223913df-203a-41e2-83a9-94343ee5d434"),
                Value = 21000
            });
            HttpJsonApiResult<PriceViewModel> contentResult = result as HttpJsonApiResult<PriceViewModel>;
            Assert.AreEqual(HttpStatusCode.Created, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task ReadSalePrice_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await productController.ReadSalePrices(new BaseApiController<Product>.PaginationQuery()
            {
                Limit = 2,
                Skip = 0
            });
            HttpJsonApiResult<IEnumerable<PriceViewModel>> contentResult = result as HttpJsonApiResult<IEnumerable<PriceViewModel>>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task CreatePurchasePrice_Success_ReturnStatusCodeCreated()
        {
            IHttpActionResult result = await productController.CreatePurchasePrice(new PriceInsertViewModel()
            {
                ProductId = new Guid("223913df-203a-41e2-83a9-94343ee5d434"),
                Value = 10000
            });
            HttpJsonApiResult<PriceViewModel> contentResult = result as HttpJsonApiResult<PriceViewModel>;
            Assert.AreEqual(HttpStatusCode.Created, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task ReadPurchasePrice_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await productController.ReadPurchasePrices(new BaseApiController<Product>.PaginationQuery()
            {
                Limit = 2,
                Skip = 0
            });
            HttpJsonApiResult<IEnumerable<PriceViewModel>> contentResult = result as HttpJsonApiResult<IEnumerable<PriceViewModel>>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }
    }
}
