using POSTWebApi.Common.Interface;
using POSTWebApi.Common.Service;
using POSTWebApi.Repository;
using POSTWebApi.Repository.Interface;
using POSTWebApi.Services.Interfaces;
using POSTWebApi.ViewModels.Entity;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace POSTWebApi.Services
{
    public abstract class BaseApiController<T> : ApiController
    {
        #region protected and private variable
        private IObjectService _objectService;
        protected IDbPOS _db = new DbManagement().db;

        #region load repository
        protected IUserRepository userRepository { get; set; } 
        protected IRoleRepository roleRepository { get; set; }
        protected ICategoryRepository categoryRepository { get; set; }
        protected IProductRepository productRepository { get; set; }
        protected ISupplierRepository supplierRepository { get; set; }
        protected IPriceRepository priceRepository { get; set; }
        protected IProductCategoryRepository productCategoryRepository { get; set; }
        protected IPurchaseRepository purchaseRepository { get; set; }
        protected IReceivedProductRepository receivedProductRepository { get; set; }
        protected ISaleRepository saleRepository { get; set; }
        #endregion

        #endregion

        #region abstract method
        public abstract Task<IHttpActionResult> Get(PaginationQuery paginationQuery);
        public abstract Task<IHttpActionResult> Get(Guid id);
        public abstract Task<IHttpActionResult> Create(T dataBody);
        public abstract Task<IHttpActionResult> Edit(Guid id, T dataBody);
        public abstract Task<IHttpActionResult> Delete(Guid id);
        #endregion

        #region public method
        public BaseApiController()
        {
            _objectService = new ObjectService<UserViewModel>();

            #region Load Repository
            userRepository = new UserRepository(_db);
            roleRepository = new RoleRepository(_db);
            categoryRepository = new CategoryRepository(_db);
            productRepository = new ProductRepository(_db);
            supplierRepository = new SupplierRepository(_db);
            priceRepository = new PriceRepository(_db);
            productCategoryRepository = new ProductCategoryRepository(_db);
            purchaseRepository = new PurchaseRepository(_db);
            receivedProductRepository = new ReceivedProductRepository(_db);
            saleRepository = new SaleRepository(_db);
            #endregion
        }
        #endregion

        #region protected method
        protected UserViewModel GetUserAuth()
        {
            return (UserViewModel)_objectService.DeserializerObject(Thread.CurrentPrincipal.Identity.Name);
        }
        #endregion

        #region nested class
        public class PaginationQuery
        {
            public int Limit { get; set; }

            public int Skip { get; set; }
        }
        #endregion
    }
}