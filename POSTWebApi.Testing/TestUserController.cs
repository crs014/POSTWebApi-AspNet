using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POSTWebApi.Controllers;
using POSTWebApi.Models;
using POSTWebApi.Services;
using POSTWebApi.Services.Interfaces;
using POSTWebApi.Testing.Services;
using POSTWebApi.ViewModels.Common;
using POSTWebApi.ViewModels.Entity;

namespace POSTWebApi.Testing
{
    [TestClass]
    public class TestUserController
    {
        private IDbPOS storeAppContext;
        private UserController userController;
        private string userToken = "1239123jknsdadasdjasjd";

        public TestUserController()
        {
            storeAppContext = new TestDbPOS();

            #region load dummy data
            UserToken userTokenObj = new UserToken()
            {
                UserId =  new Guid("c3f00ad7-afdb-441e-ab37-ec12dc0fefc5"),
                Token = userToken,
                DeviceNumber = "PTX123"
            };
            User userDataLogin = new User()
            {
                Id = new Guid("c3f00ad7-afdb-441e-ab37-ec12dc0fefc5"),
                Email = "superuser@su",
                Password = "123456",
                Name = "superuser",
                Phone = "000000000",
                Gender = GenderEnum.Male,
                UserRoles = new List<UserRole>()
                {
                    new UserRole()
                    {
                        UserId = new Guid("c3f00ad7-afdb-441e-ab37-ec12dc0fefc5"),
                        RoleId = new Guid("34163b07-0cf8-49d4-893a-9ffe20496d3d"),
                        Role = new Role()
                        {
                             Id = new Guid("34163b07-0cf8-49d4-893a-9ffe20496d3d"),
                            Name = "superuser"
                        }
                    }
                },
                UserTokens = new List<UserToken>() { userTokenObj }
            };
            storeAppContext.Users.Add(userDataLogin);
            storeAppContext.Users.Add(new User()
            {
                Id = new Guid("fbc41aa2-b12e-4494-99ba-711278de74c7"),
                Email = "johndoe@mail",
                Password = "123456",
                Name = "john doe",
                Phone = "12345678",
                Gender = GenderEnum.Male,
                UserRoles = new List<UserRole>()
                {
                    new UserRole()
                    {
                        UserId = new Guid("c3f00ad7-afdb-441e-ab37-ec12dc0fefc5"),
                        RoleId = new Guid("0709bfc4-5e9e-436f-a5ee-dd93162633b4"),
                        Role = new Role()
                        {
                            Id = new Guid("0709bfc4-5e9e-436f-a5ee-dd93162633b4"),
                            Name = "Customer"
                        }
                    }
                }
            });
            storeAppContext.UserTokens.Add(userTokenObj);
            #endregion

            #region Load Controller
            userController = new UserController(storeAppContext, userDataLogin);
            #endregion
        }

        [TestMethod]
        public async Task ReadUserList_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await userController.Get(new BaseApiController<User>.PaginationQuery()
            {
                Limit = 2,
                Skip = 0
            });
            HttpJsonApiResult<IEnumerable<UserViewModel>> contentResult = result as HttpJsonApiResult<IEnumerable<UserViewModel>>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task LoginUser_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await userController.Login(new LoginViewModel()
            {
                Email = "superuser@su",
                Password = "123456",
                DeviceNumber = "PTX123"
            });
            HttpJsonApiResult<TokenViewModel> contentResult = result as HttpJsonApiResult<TokenViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task LoginUser_UserNotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await userController.Login(new LoginViewModel()
            {
                Email = "super@su",
                Password = "123456",
                DeviceNumber = "PTX123"
            });
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task Logout_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await userController.Logout(new WebTokenViewModel()
            {
                Token = userToken
            });
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task Logout_Fail_ReturnStatusCodeBadRequest()
        {
            IHttpActionResult result = await userController.Logout(new WebTokenViewModel()
            {
                Token = "adas122131aasf"
            });
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.BadRequest, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task LogoutAll_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await userController.LogoutAll();
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task RefreshToken_Success_ReturnStatusCodeOk()
        {
            //login again
            IHttpActionResult result = await userController.Login(new LoginViewModel()
            {
                Email = "superuser@su",
                Password = "123456",
                DeviceNumber = "PTX123"
            });
            HttpJsonApiResult<TokenViewModel> contentResult = result as HttpJsonApiResult<TokenViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);


            //refresh token
            IHttpActionResult result2 = await userController.RefreshToken(new WebTokenViewModel() {
                Token = contentResult.DataValue.RefreshToken
            });
            HttpJsonApiResult<TokenViewModel> contentResult2 = result2 as HttpJsonApiResult<TokenViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult2.StatusCode);
        }

        [TestMethod]
        public async Task RefreshToken_Fail_ReturnStatusCodeBadRequest()
        {
            IHttpActionResult result = await userController.RefreshToken(new WebTokenViewModel(){
                Token = "as1212sdals1201lemasmd"
            });
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.BadRequest, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task CreateUser_Success_ReturnStatusCodeCreated()
        {
            IHttpActionResult result = await userController.Create(new User()
            {
                Email = "uknown@mail",
                Password = "123456",
                Name = "Name",
                Phone = "2566778900",
                Gender = GenderEnum.Male
            });
            HttpJsonApiResult<UserViewModel> resultContent = result as HttpJsonApiResult<UserViewModel>;
            Assert.AreEqual(HttpStatusCode.Created, resultContent.StatusCode);
        }

        [TestMethod]
        public async Task EditUser_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await userController.Edit(new Guid("fbc41aa2-b12e-4494-99ba-711278de74c7"), new User()
            {
                Email = "johndoe@mail",
                Password = "123456",
                Name = "john doe",
                Phone = "12345678",
                Gender = GenderEnum.Male,
            });
            HttpJsonApiResult<UserViewModel> contentResult = result as HttpJsonApiResult<UserViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task EditUser_UserNotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await userController.Edit(new Guid("ba740d49-49f8-4fce-a973-df7dc1a1cbc7"), new User()
            {
                Email = "Uknown@gmail",
                Password = "123456",
                Name = "uknown",
                Phone = "123123312",
                Gender = GenderEnum.Male,
            });
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task SoftDeleteUser_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await userController.Delete(new Guid("fbc41aa2-b12e-4494-99ba-711278de74c7"));
            HttpJsonApiResult<UserViewModel> contentResult = result as HttpJsonApiResult<UserViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task SoftDeleteUser_UserNotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await userController.Delete(new Guid("ba740d49-49f8-4fce-a973-df7dc1a1cbc7"));
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task HardDeleteUser_Success_ReturnStatusCodeOk()
        {
            IHttpActionResult result = await userController.HardDeleteUser(new Guid("fbc41aa2-b12e-4494-99ba-711278de74c7"));
            HttpJsonApiResult<UserViewModel> contentResult = result as HttpJsonApiResult<UserViewModel>;
            Assert.AreEqual(HttpStatusCode.OK, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task HardDeleteUser_UserNotFound_ReturnStatusCodeNotFound()
        {
            IHttpActionResult result = await userController.HardDeleteUser(new Guid("ba740d49-49f8-4fce-a973-df7dc1a1cbc7"));
            HttpJsonApiResult<string> contentResult = result as HttpJsonApiResult<string>;
            Assert.AreEqual(HttpStatusCode.NotFound, contentResult.StatusCode);
        }
    }
}
