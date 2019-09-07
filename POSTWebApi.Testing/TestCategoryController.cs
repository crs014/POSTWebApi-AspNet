using Microsoft.VisualStudio.TestTools.UnitTesting;
using POSTWebApi.Controllers;
using POSTWebApi.Models;
using POSTWebApi.Services;
using POSTWebApi.Services.Interfaces;
using POSTWebApi.Testing.Services;
using POSTWebApi.ViewModels.Entity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace POSTWebApi.Testing
{
    [TestClass]
    public class TestCategoryController
    {
        private IDbPOS storeAppContext;
        private CategoryController categoryController;

        public TestCategoryController()
        {
            #region load dummy data
            storeAppContext = new TestDbPOS();
            storeAppContext.Categories.Add(new Category()
            {
                Id = new Guid("fbc41aa2-b12e-4494-99ba-711278de74c7"),
                Name = "Weapon"
            });
            storeAppContext.Categories.Add(new Category()
            {
                Id = new Guid("c3f00ad7-afdb-441e-ab37-ec12dc0fefc5"),
                Name = "Armor"
            });
            #endregion

            #region load controller
            categoryController = new CategoryController(storeAppContext);
            #endregion
        }

        [TestMethod]
        public async Task ReadCategoryList_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await categoryController.Get(new BaseApiController<Category>.PaginationQuery()
            {
                Limit = 2,
                Skip = 0
            });
            HttpJsonApiResult<IEnumerable<CategoryViewModel>> contentResult = result as HttpJsonApiResult<IEnumerable<CategoryViewModel>>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task ReadCategory_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await categoryController.Get(new Guid("fbc41aa2-b12e-4494-99ba-711278de74c7"));
            HttpJsonApiResult<CategoryViewModel> contentResult = result as HttpJsonApiResult<CategoryViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task ReadCategory_NotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await categoryController.Get(Guid.NewGuid());
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task CreateCategory_Success_ReturnStatusCodeCreated()
        {
            IHttpActionResult result = await categoryController.Create(new Category()
            {
                Name = "Potion"
            });
            HttpJsonApiResult<CategoryViewModel> contentResult = result as HttpJsonApiResult<CategoryViewModel>;
            Assert.AreEqual(HttpStatusCode.Created, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task EditCategory_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await categoryController.Edit(
                new Guid("fbc41aa2-b12e-4494-99ba-711278de74c7"),
                new Category()
                {
                    Name = "Gun"
                }
            );
            HttpJsonApiResult<CategoryViewModel> contentResult = result as HttpJsonApiResult<CategoryViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task EditCategory_NotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await categoryController.Edit(Guid.NewGuid(), new Category()
            {
                Name = "Gun"
            });
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task SoftDeleteCategory_Success_ReturnStatusOk()
        {
            IHttpActionResult result = await categoryController.Delete(new Guid("fbc41aa2-b12e-4494-99ba-711278de74c7"));
            HttpJsonApiResult<CategoryViewModel> contentResult = result as HttpJsonApiResult<CategoryViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task SoftDeleteCategory_NotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await categoryController.Delete(Guid.NewGuid());
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task HardDeleteCategory_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await categoryController.HardDeleteProduct(new Guid("fbc41aa2-b12e-4494-99ba-711278de74c7"));
            HttpJsonApiResult<CategoryViewModel> contentResult = result as HttpJsonApiResult<CategoryViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task HardDeleteCategory_NotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await categoryController.HardDeleteProduct(Guid.NewGuid());
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }
    }
}
