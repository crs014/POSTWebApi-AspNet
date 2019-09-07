using Microsoft.VisualStudio.TestTools.UnitTesting;
using POSTWebApi.Controllers;
using POSTWebApi.Models;
using POSTWebApi.Services;
using POSTWebApi.Services.Interfaces;
using POSTWebApi.Testing.Services;
using POSTWebApi.ViewModels.Entity;
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
    public class TestReceivedProductController
    {
        private IDbPOS storeAppContext;
        private ReceivedProductController receivedProductController;

        public TestReceivedProductController()
        {
            #region load dummy data
            storeAppContext = new TestDbPOS();
            User user = new User()
            {
                Id = new Guid("839b0da7-50f7-4af0-98bf-4bd746cfa192"),
                Email = "johndoe@mail",
                Name = "john doe",
                Gender = GenderEnum.Male,
                Password = "123456",
                UserRoles = new List<UserRole>()
            };
            storeAppContext.ReceivedProducts.Add(new ReceivedProduct()
            {
                Id = new Guid("839b0da7-50f7-4af0-98bf-4bd746cfa192"),
                ProductDetailID = new Guid("3bc4d343-1a0d-432f-a190-d8f76ebb1ab9"),
                UserId = new Guid("839b0da7-50f7-4af0-98bf-4bd746cfa192"),
                Quantity = 10,
                User = user,
                ProductDetail = new ProductDetail()
            });
            storeAppContext.ReceivedProducts.Add(new ReceivedProduct()
            {
                Id = new Guid("5b13ea1f-dfb9-4f56-9636-b45a8036d7ca"),
                ProductDetailID = new Guid("ed08a51b-8e08-4e71-8b28-db1ec5ce3d0b"),
                UserId = new Guid("5b13ea1f-dfb9-4f56-9636-b45a8036d7ca"),
                Quantity = 10,
                User = new User()
                {
                    Id = new Guid("5b13ea1f-dfb9-4f56-9636-b45a8036d7ca"),
                    UserRoles = new List<UserRole>()
                },
                ProductDetail = new ProductDetail()
            });
            #endregion

            #region load controller
            receivedProductController = new ReceivedProductController(storeAppContext, user);
            #endregion
        }

        [TestMethod]
        public async Task ReadReceivedProductList_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await receivedProductController.Get(new BaseApiController<ReceivedProduct>.PaginationQuery
            {
                Limit = 2,
                Skip = 0
            });
            HttpJsonApiResult<IEnumerable<ReceivedProductViewModel>> contentResult = result as HttpJsonApiResult<IEnumerable<ReceivedProductViewModel>>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task ReadReceivedProduct_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await receivedProductController.Get(new Guid("3bc4d343-1a0d-432f-a190-d8f76ebb1ab9"));
            HttpJsonApiResult<IEnumerable<ReceivedProductViewModel>> contentResult = result as HttpJsonApiResult<IEnumerable<ReceivedProductViewModel>>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task ReadReceivedProduct_NotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await receivedProductController.Get(Guid.NewGuid());
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task EditReceivedProduct_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await receivedProductController.Edit(
                new Guid("839b0da7-50f7-4af0-98bf-4bd746cfa192"), new ReceivedProduct());
            HttpJsonApiResult<ReceivedProductViewModel> contentResult = result as HttpJsonApiResult<ReceivedProductViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task EditReceivedProduct_NotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await receivedProductController.Edit(Guid.NewGuid(), new ReceivedProduct());
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task DeleteReceivedProduct_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await receivedProductController.Delete(new Guid("839b0da7-50f7-4af0-98bf-4bd746cfa192"));
            HttpJsonApiResult<ReceivedProductViewModel> contentResult = result as HttpJsonApiResult<ReceivedProductViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task DeleteReceivedProduct_NotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await receivedProductController.Delete(Guid.NewGuid());
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }
    }
}
