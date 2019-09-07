using POSTWebApi.Models;
using POSTWebApi.Services.Interfaces;
using POSTWebApi.Testing.DbSet;
using System.Data.Entity;

namespace POSTWebApi.Testing.Services
{
    class TestDbPOS : IDbPOS
    {
        public TestDbPOS()
        {
            Users = new TestUserDbSet();
            Roles = new TestRoleDbSet();
            UserRoles = new TestUserRoleDbSet();
            UserTokens = new TestUserTokenDbSet();
            ReceivedProducts = new TestReceivedProductDbSet();
            Features = new TestFeatureDbSet();
            Suppliers = new TestSupplierDbSet();
            Products = new TestProductDbSet();
            Categories = new TestCategoryDbSet();
            Prices = new TestPriceDbSet();
            ProductCategories = new TestProductCategoryDbSet();
            ProductImages = new TestProductImageDbSet();
            RoleFeatures = new TestRoleFeatureDbSet();
            Purchases = new TestPurchaseDbSet();
            ProductDetails = new TestProductDetailDbSet();
            Sales = new TestSaleDbSet();
            SaleDetails = new TestSaleDetailDbSet();
            SaleRequests = new TestSaleRequestDbSet();
        }

        public DbSet<User> Users { get; set; } 
    
        public DbSet<Feature> Features { get; set; }

        public DbSet<Supplier> Suppliers { get; set; } 

        public DbSet<Product> Products { get; set; } 

        public DbSet<Category> Categories { get; set; } 

        public DbSet<Role> Roles { get; set; } 

        public DbSet<Price> Prices { get; set; } 

        public DbSet<ProductCategory> ProductCategories { get; set; } 

        public DbSet<ProductImage> ProductImages { get; set; } 

        public DbSet<UserToken> UserTokens { get; set; } 
   
        public DbSet<UserRole> UserRoles { get; set; } 

        public DbSet<RoleFeature> RoleFeatures { get; set; } 

        public DbSet<Purchase> Purchases { get; set; } 

        public DbSet<ProductDetail> ProductDetails { get; set; } 
   
        public DbSet<ReceivedProduct> ReceivedProducts { get; set; } 

        public DbSet<Sale> Sales { get; set; }

        public DbSet<SaleComplaint> SaleComplaints { get; set; }
    
        public DbSet<SaleDetail> SaleDetails { get; set; }

        public DbSet<SaleRequest> SaleRequests { get; set; }

        public Database Database { get; }

        public void Dispose()
        {
            
        }

        public void MarkAsModified(User user)
        {
            
        }

        public int SaveChanges()
        {
            return 0;
        }
    }
}
