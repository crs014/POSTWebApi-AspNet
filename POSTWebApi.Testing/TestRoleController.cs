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
    public class TestRoleController
    {
        private IDbPOS storeAppContext;
        private RoleController roleController;

        public TestRoleController()
        {
            #region load dummy
            storeAppContext = new TestDbPOS();
            storeAppContext.Roles.Add(new Role() {
                Id = new Guid("223913df-203a-41e2-83a9-94343ee5d434"),
                Name = "superuser"
            });
            storeAppContext.Roles.Add(new Role()
            {
                Id = new Guid("5579b911-2392-43f8-87cf-83e1a7bbed6f"),
                Name = "admin"
            });
            #endregion

            #region load controller
            roleController = new RoleController(storeAppContext);
            #endregion
        }

        [TestMethod]
        public async Task ReadRoleList_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await roleController.Get(new BaseApiController<Role>.PaginationQuery()
            {
                Limit = 2,
                Skip = 0
            });
            HttpJsonApiResult<IEnumerable<RoleViewModel>> contentResult = result as HttpJsonApiResult<IEnumerable<RoleViewModel>>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task ReadRole_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await roleController.Get(new Guid("223913df-203a-41e2-83a9-94343ee5d434"));
            HttpJsonApiResult<RoleViewModel> contentResult = result as HttpJsonApiResult<RoleViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task ReadRole_NotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await roleController.Get(Guid.NewGuid());
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task CreateRole_Success_ReturnStatusCodeCreated()
        {
            IHttpActionResult result = await roleController.Create(new Role()
            {
                Name = "manager"
            });
            HttpJsonApiResult<RoleViewModel> contentResult = result as HttpJsonApiResult<RoleViewModel>;
            Assert.AreEqual(HttpStatusCode.Created, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task EditRole_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await roleController.Edit(new Guid("5579b911-2392-43f8-87cf-83e1a7bbed6f"), new Role()
            {
                Name = "store admin"
            });
            HttpJsonApiResult<RoleViewModel> contentResult = result as HttpJsonApiResult<RoleViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task EditRole_NotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await roleController.Edit(Guid.NewGuid(), new Role()
            {
                Name = "store admin"
            });
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task SoftDeleteRole_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await roleController.Delete(new Guid("5579b911-2392-43f8-87cf-83e1a7bbed6f"));
            HttpJsonApiResult<RoleViewModel> contentResult = result as HttpJsonApiResult<RoleViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task SoftDeleteRole_NotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await roleController.Delete(Guid.NewGuid());
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task HardDeleteRole_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await roleController.HardDeleteRole(new Guid("5579b911-2392-43f8-87cf-83e1a7bbed6f"));
            HttpJsonApiResult<RoleViewModel> contentResult = result as HttpJsonApiResult<RoleViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task HardDeleteRole_NotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await roleController.HardDeleteRole(Guid.NewGuid());
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }

    }
}
