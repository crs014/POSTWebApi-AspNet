using POSTWebApi.Models;
using POSTWebApi.Services.Interfaces;
using System.Data.Entity;
namespace POSTWebApi.Services
{
    public class DbPOS : DbContext, IDbPOS
    {

        public DbPOS() : base("name=DbPOS")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public void MarkAsModified(User user)
        {
            Entry(user).State = EntityState.Modified;
        }

        public virtual DbSet<User> Users { get; set; } 
        public virtual DbSet<Feature> Features { get; set; } 
        public virtual DbSet<Supplier> Suppliers { get; set; } 
        public virtual DbSet<Product> Products { get; set; } 
        public virtual DbSet<Category> Categories { get; set; } 
        public virtual DbSet<Role> Roles { get; set; } 
        public virtual DbSet<Price> Prices { get; set; } 
        public virtual DbSet<ProductCategory> ProductCategories { get; set; } 
        public virtual DbSet<ProductImage> ProductImages { get; set; } 
        public virtual DbSet<UserToken> UserTokens { get; set; } 
        public virtual DbSet<UserRole> UserRoles { get; set; } 
        public virtual DbSet<RoleFeature> RoleFeatures { get; set; } 
        public virtual DbSet<Purchase> Purchases { get; set; } 
        public virtual DbSet<ProductDetail> ProductDetails { get; set; } 
        public virtual DbSet<ReceivedProduct> ReceivedProducts { get; set; } 
        public virtual DbSet<Sale> Sales { get; set; } 
        public virtual DbSet<SaleComplaint> SaleComplaints { get; set; } 
        public virtual DbSet<SaleDetail> SaleDetails { get; set; }
        public virtual DbSet<SaleRequest> SaleRequests { get; set; }
    }
}