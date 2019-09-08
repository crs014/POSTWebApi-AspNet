namespace POSTWebApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SecondInitial : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserTokens", "DeviceNumber", c => c.String(nullable: false, maxLength: 255, unicode: false));
            CreateIndex("dbo.UserTokens", "DeviceNumber", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserTokens", new[] { "DeviceNumber" });
            AlterColumn("dbo.UserTokens", "DeviceNumber", c => c.String(nullable: false, unicode: false, storeType: "text"));
        }
    }
}
