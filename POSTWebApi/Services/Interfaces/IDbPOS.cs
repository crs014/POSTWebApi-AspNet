using POSTWebApi.Models;
using System;
using System.Data.Entity;

namespace POSTWebApi.Services.Interfaces
{
    public interface IDbPOS : IDisposable
    {
        #region DbSet
        DbSet<User> Users { get; }
        DbSet<Feature> Features { get; }
        DbSet<Supplier> Suppliers { get; }
        DbSet<Product> Products { get; }
        DbSet<Category> Categories { get; }
        DbSet<Role> Roles { get; }
        DbSet<Price> Prices { get; }
        DbSet<ProductCategory> ProductCategories { get; }
        DbSet<ProductImage> ProductImages { get; }
        DbSet<UserToken> UserTokens { get; }
        DbSet<UserRole> UserRoles { get; }
        DbSet<RoleFeature> RoleFeatures { get; }
        DbSet<Purchase> Purchases { get; }
        DbSet<ProductDetail> ProductDetails { get; }
        DbSet<ReceivedProduct> ReceivedProducts { get; }
        DbSet<Sale> Sales { get; }
        DbSet<SaleComplaint> SaleComplaints { get; }
        DbSet<SaleDetail> SaleDetails { get; }
        DbSet<SaleRequest> SaleRequests { get; }
        #endregion

        #region variable
        Database Database { get; }
        #endregion

        #region method
        int SaveChanges();
        void MarkAsModified(User user);
        #endregion
    }
}
