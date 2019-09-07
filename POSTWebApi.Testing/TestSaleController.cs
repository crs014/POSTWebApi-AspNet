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
    public class TestSaleController
    {
        private IDbPOS storeAppContext;
        private SaleController saleController;

        public TestSaleController()
        {
            #region load dummy data
            storeAppContext = new TestDbPOS();
            storeAppContext.Sales.Add(new Sale()
            {
                Id = new Guid("3bc4d343-1a0d-432f-a190-d8f76ebb1ab9"),
                UserId = new Guid("839b0da7-50f7-4af0-98bf-4bd746cfa192"),
                User = new User()
                {
                    UserRoles = new List<UserRole>()
                }
            });
            storeAppContext.Sales.Add(new Sale()
            {
                Id = new Guid("ed08a51b-8e08-4e71-8b28-db1ec5ce3d0b"),
                UserId = new Guid("5b13ea1f-dfb9-4f56-9636-b45a8036d7ca"),
                User = new User()
                {
                    UserRoles = new List<UserRole>()
                }
            });
            #endregion

            #region load controller
            saleController = new SaleController(storeAppContext);
            #endregion
        }

        [TestMethod]
        public async Task ReadSaleList_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await saleController.Get(new BaseApiController<Sale>.PaginationQuery
            {
                Limit = 2,
                Skip = 0
            });
            HttpJsonApiResult<IEnumerable<SaleViewModel>> contentResult = result as HttpJsonApiResult<IEnumerable<SaleViewModel>>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task ReadSale_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await saleController.Get(new Guid("3bc4d343-1a0d-432f-a190-d8f76ebb1ab9"));
            HttpJsonApiResult<SaleDetailViewModel> contentResult = result as HttpJsonApiResult<SaleDetailViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task ReadSale_NotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await saleController.Get(Guid.NewGuid());
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task EditSale_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await saleController.Edit(new Guid("3bc4d343-1a0d-432f-a190-d8f76ebb1ab9"), new Sale());
            HttpJsonApiResult<SaleViewModel> contentResult = result as HttpJsonApiResult<SaleViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task EditSale_NotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await saleController.Edit(Guid.NewGuid(), new Sale());
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task SoftDeleteSale_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await saleController.Delete(new Guid("3bc4d343-1a0d-432f-a190-d8f76ebb1ab9"));
            HttpJsonApiResult<SaleViewModel> contentResult = result as HttpJsonApiResult<SaleViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task SoftDeleteSale_NotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await saleController.Delete(Guid.NewGuid());
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task HardDeleteSale_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await saleController.HardDelete(new Guid("3bc4d343-1a0d-432f-a190-d8f76ebb1ab9"));
            HttpJsonApiResult<SaleViewModel> contentResult = result as HttpJsonApiResult<SaleViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task HardDeleteSale_NotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await saleController.HardDelete(Guid.NewGuid());
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }
    }
}
