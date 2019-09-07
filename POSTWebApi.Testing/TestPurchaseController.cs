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
    public class TestPurchaseController
    {
        private IDbPOS storeAppContext;
        private PurchaseController purchaseController;

        public TestPurchaseController()
        {
            #region load dummy data
            storeAppContext = new TestDbPOS();
            storeAppContext.Purchases.Add(new Purchase()
            {
                Id = new Guid("3bc4d343-1a0d-432f-a190-d8f76ebb1ab9"),
                SupplierId = new Guid("890f2f7c-5442-4c8b-aedf-9baa6839fc78"),
                UserId = new Guid("839b0da7-50f7-4af0-98bf-4bd746cfa192"),
                Supplier = new Supplier(),
                User = new User()
                {
                    UserRoles = new List<UserRole>()
                }
            });
            storeAppContext.Purchases.Add(new Purchase()
            {
                Id = new Guid("ed08a51b-8e08-4e71-8b28-db1ec5ce3d0b"),
                SupplierId = new Guid("e349b832-46da-4543-a3eb-9e697f7207e3"),
                UserId = new Guid("5b13ea1f-dfb9-4f56-9636-b45a8036d7ca"),
                Supplier = new Supplier(),
                User = new User()
                {
                    UserRoles = new List<UserRole>()
                }
            });
            #endregion

            #region load controller
            purchaseController = new PurchaseController(storeAppContext);
            #endregion
        }

        [TestMethod]
        public async Task ReadPurchaseList_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await purchaseController.Get(new BaseApiController<Purchase>.PaginationQuery
            {
                Limit = 2,
                Skip = 0
            });
            HttpJsonApiResult<IEnumerable<PurchaseViewModel>> contentResult = result as HttpJsonApiResult<IEnumerable<PurchaseViewModel>>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task ReadPurchase_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await purchaseController.Get(new Guid("3bc4d343-1a0d-432f-a190-d8f76ebb1ab9"));
            HttpJsonApiResult<PurchaseDetailViewModel> contentResult = result as HttpJsonApiResult<PurchaseDetailViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task ReadPurchase_NotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await purchaseController.Get(Guid.NewGuid());
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task EditPurchase_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await purchaseController.Edit(new Guid("3bc4d343-1a0d-432f-a190-d8f76ebb1ab9"), new Purchase());
            HttpJsonApiResult<PurchaseViewModel> contentResult = result as HttpJsonApiResult<PurchaseViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task EditPurchase_NotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await purchaseController.Edit(Guid.NewGuid(), new Purchase());
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task SoftDeletePurchase_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await purchaseController.Delete(new Guid("3bc4d343-1a0d-432f-a190-d8f76ebb1ab9"));
            HttpJsonApiResult<PurchaseViewModel> contentResult = result as HttpJsonApiResult<PurchaseViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task SoftDeletePurchase_NotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await purchaseController.Delete(Guid.NewGuid());
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task HardDeletePurchase_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await purchaseController.HardDeleteProduct(new Guid("3bc4d343-1a0d-432f-a190-d8f76ebb1ab9"));
            HttpJsonApiResult<PurchaseViewModel> contentResult = result as HttpJsonApiResult<PurchaseViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task HardDeletePurchase_NotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await purchaseController.HardDeleteProduct(Guid.NewGuid());
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }
    }
}
