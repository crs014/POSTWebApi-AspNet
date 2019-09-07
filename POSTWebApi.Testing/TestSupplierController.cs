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
    public class TestSupplierController
    {
        private IDbPOS storeAppContext;
        private SupplierController supplierController;

        public TestSupplierController()
        {
            #region load dummy
            storeAppContext = new TestDbPOS();
            storeAppContext.Suppliers.Add(new Supplier()
            {
                Id = new Guid("bbbf396b-5b78-4924-bd21-a9afa9decfc8"),
                Name = "PT Santuy Selalu",
                Address = "Jalan Santuy Blok S",
                Phone = "0799797979",
                Description = "Santuy Terus ya"
            });
            storeAppContext.Suppliers.Add(new Supplier()
            {
                Id = new Guid("bd12976a-bfdb-436f-b790-618399da434e"),
                Name = "PT Cucok Bagus",
                Address = "Jalan Lekong Blok L",
                Phone = "07970012122",
                Description = "Lekong ya cik"
            });
            #endregion

            #region load controller
            supplierController = new SupplierController(storeAppContext);
            #endregion
        }

        [TestMethod]
        public async Task ReadSupplierList_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await supplierController.Get(new BaseApiController<Supplier>.PaginationQuery()
            {
                Limit = 2,
                Skip = 0
            });
            HttpJsonApiResult<IEnumerable<SupplierViewModel>> contentResult = result as HttpJsonApiResult<IEnumerable<SupplierViewModel>>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task ReadSupplier_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await supplierController.Get(new Guid("bbbf396b-5b78-4924-bd21-a9afa9decfc8"));
            HttpJsonApiResult<SupplierViewModel> contentResult = result as HttpJsonApiResult<SupplierViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task ReadSupplier_NotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await supplierController.Get(Guid.NewGuid());
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task CreateSupplier_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await supplierController.Create(new Supplier()
            {
                Name = "PT Tambah lagi",
                Address = "Tambah Lagi",
                Phone = "868686866",
                Description = "cuma test create"
            });
            HttpJsonApiResult<SupplierViewModel> contentResult = result as HttpJsonApiResult<SupplierViewModel>;
            Assert.AreEqual(HttpStatusCode.Created, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task EditSupplier_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await supplierController.Edit(new Guid("bbbf396b-5b78-4924-bd21-a9afa9decfc8"), new Supplier()
            {
                Name = "PT Tukar",
                Address = "tukar lagi",
                Phone = "8686868667112",
                Description = "cuma test tukar"
            });
            HttpJsonApiResult<SupplierViewModel> contentResult = result as HttpJsonApiResult<SupplierViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task EditSupplier_NotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await supplierController.Edit(Guid.NewGuid(), new Supplier()
            {
                Name = "PT Tukar",
                Address = "tukar lagi",
                Phone = "8686868667112",
                Description = "cuma test tukar"
            });
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task SoftDeleteSupplier_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await supplierController.Delete(new Guid("bbbf396b-5b78-4924-bd21-a9afa9decfc8"));
            HttpJsonApiResult<SupplierViewModel> contentResult = result as HttpJsonApiResult<SupplierViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task SoftDeleteSupplier_NotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await supplierController.Delete(Guid.NewGuid());
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task HardDeleteSupplier_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await supplierController.HardDeleteSupplier(new Guid("bbbf396b-5b78-4924-bd21-a9afa9decfc8"));
            HttpJsonApiResult<SupplierViewModel> contentResult = result as HttpJsonApiResult<SupplierViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task HardDeleteSupplier_NotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await supplierController.HardDeleteSupplier(Guid.NewGuid());
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }
    }
}
