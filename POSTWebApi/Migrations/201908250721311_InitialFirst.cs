namespace POSTWebApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialFirst : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 45),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, storeType: "rowversion"),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductCategories",
                c => new
                    {
                        ProductId = c.Guid(nullable: false),
                        CategoryId = c.Guid(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => new { t.ProductId, t.CategoryId })
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 45, unicode: false),
                        Description = c.String(maxLength: 255, unicode: false),
                        CodeProduct = c.String(nullable: false, maxLength: 45, unicode: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, storeType: "rowversion"),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.CodeProduct, unique: true);
            
            CreateTable(
                "dbo.Prices",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ProductId = c.Guid(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Type = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.ProductDetails",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ProductId = c.Guid(nullable: false),
                        PurchaseId = c.Guid(nullable: false),
                        Quantity = c.Int(nullable: false),
                        CodeProductDetail = c.String(),
                        Expired = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, storeType: "rowversion"),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Purchases", t => t.PurchaseId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.PurchaseId);
            
            CreateTable(
                "dbo.Purchases",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        SupplierId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, storeType: "rowversion"),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.SupplierId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 45),
                        Address = c.String(maxLength: 45),
                        Phone = c.String(nullable: false),
                        Description = c.String(maxLength: 45),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, storeType: "rowversion"),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 255, unicode: false),
                        Password = c.String(nullable: false, maxLength: 45),
                        Name = c.String(nullable: false, maxLength: 45),
                        Phone = c.String(maxLength: 45, unicode: false),
                        Gender = c.Int(nullable: false),
                        Avatar = c.Binary(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, storeType: "rowversion"),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Email, unique: true)
                .Index(t => t.Phone, unique: true);
            
            CreateTable(
                "dbo.SaleRequests",
                c => new
                    {
                        SaleId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        Address = c.String(nullable: false, maxLength: 255),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => new { t.SaleId, t.UserId })
                .ForeignKey("dbo.Sales", t => t.SaleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.SaleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        UserId = c.Guid(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, storeType: "rowversion"),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.SaleComplaints",
                c => new
                    {
                        SaleId = c.Guid(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 255),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.SaleId)
                .ForeignKey("dbo.Sales", t => t.SaleId)
                .Index(t => t.SaleId);
            
            CreateTable(
                "dbo.SaleDetails",
                c => new
                    {
                        SaleId = c.Guid(nullable: false),
                        ProductDetailId = c.Guid(nullable: false),
                        Quantity = c.Int(nullable: false),
                        DateTimeReceived = c.DateTime(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.SaleId, t.ProductDetailId })
                .ForeignKey("dbo.ProductDetails", t => t.ProductDetailId, cascadeDelete: true)
                .ForeignKey("dbo.Sales", t => t.SaleId, cascadeDelete: true)
                .Index(t => t.SaleId)
                .Index(t => t.ProductDetailId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 45),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, storeType: "rowversion"),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name);
            
            CreateTable(
                "dbo.RoleFeatures",
                c => new
                    {
                        FeatureId = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => new { t.FeatureId, t.RoleId })
                .ForeignKey("dbo.Features", t => t.FeatureId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.FeatureId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Features",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 45),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, storeType: "rowversion"),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserTokens",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        UserId = c.Guid(nullable: false),
                        DeviceNumber = c.String(nullable: false, unicode: false, storeType: "text"),
                        Token = c.String(nullable: false, unicode: false, storeType: "text"),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ReceivedProducts",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ProductDetailID = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        Quantity = c.Int(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, storeType: "rowversion"),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductDetails", t => t.ProductDetailID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.ProductDetailID)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ProductImages",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ProductId = c.Guid(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, storeType: "rowversion"),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductImages", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ReceivedProducts", "UserId", "dbo.Users");
            DropForeignKey("dbo.ReceivedProducts", "ProductDetailID", "dbo.ProductDetails");
            DropForeignKey("dbo.UserTokens", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RoleFeatures", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RoleFeatures", "FeatureId", "dbo.Features");
            DropForeignKey("dbo.SaleRequests", "UserId", "dbo.Users");
            DropForeignKey("dbo.Sales", "UserId", "dbo.Users");
            DropForeignKey("dbo.SaleRequests", "SaleId", "dbo.Sales");
            DropForeignKey("dbo.SaleDetails", "SaleId", "dbo.Sales");
            DropForeignKey("dbo.SaleDetails", "ProductDetailId", "dbo.ProductDetails");
            DropForeignKey("dbo.SaleComplaints", "SaleId", "dbo.Sales");
            DropForeignKey("dbo.Purchases", "UserId", "dbo.Users");
            DropForeignKey("dbo.Purchases", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.ProductDetails", "PurchaseId", "dbo.Purchases");
            DropForeignKey("dbo.ProductDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductCategories", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Prices", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductCategories", "CategoryId", "dbo.Categories");
            DropIndex("dbo.ProductImages", new[] { "ProductId" });
            DropIndex("dbo.ReceivedProducts", new[] { "UserId" });
            DropIndex("dbo.ReceivedProducts", new[] { "ProductDetailID" });
            DropIndex("dbo.UserTokens", new[] { "UserId" });
            DropIndex("dbo.RoleFeatures", new[] { "RoleId" });
            DropIndex("dbo.RoleFeatures", new[] { "FeatureId" });
            DropIndex("dbo.Roles", new[] { "Name" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.SaleDetails", new[] { "ProductDetailId" });
            DropIndex("dbo.SaleDetails", new[] { "SaleId" });
            DropIndex("dbo.SaleComplaints", new[] { "SaleId" });
            DropIndex("dbo.Sales", new[] { "UserId" });
            DropIndex("dbo.SaleRequests", new[] { "UserId" });
            DropIndex("dbo.SaleRequests", new[] { "SaleId" });
            DropIndex("dbo.Users", new[] { "Phone" });
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.Purchases", new[] { "UserId" });
            DropIndex("dbo.Purchases", new[] { "SupplierId" });
            DropIndex("dbo.ProductDetails", new[] { "PurchaseId" });
            DropIndex("dbo.ProductDetails", new[] { "ProductId" });
            DropIndex("dbo.Prices", new[] { "ProductId" });
            DropIndex("dbo.Products", new[] { "CodeProduct" });
            DropIndex("dbo.ProductCategories", new[] { "CategoryId" });
            DropIndex("dbo.ProductCategories", new[] { "ProductId" });
            DropTable("dbo.ProductImages");
            DropTable("dbo.ReceivedProducts");
            DropTable("dbo.UserTokens");
            DropTable("dbo.Features");
            DropTable("dbo.RoleFeatures");
            DropTable("dbo.Roles");
            DropTable("dbo.UserRoles");
            DropTable("dbo.SaleDetails");
            DropTable("dbo.SaleComplaints");
            DropTable("dbo.Sales");
            DropTable("dbo.SaleRequests");
            DropTable("dbo.Users");
            DropTable("dbo.Suppliers");
            DropTable("dbo.Purchases");
            DropTable("dbo.ProductDetails");
            DropTable("dbo.Prices");
            DropTable("dbo.Products");
            DropTable("dbo.ProductCategories");
            DropTable("dbo.Categories");
        }
    }
}
