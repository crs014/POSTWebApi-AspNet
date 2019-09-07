namespace POSTWebApi.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using POSTWebApi.Models;
    using POSTWebApi.Services;

    internal sealed class Configuration : DbMigrationsConfiguration<POSTWebApi.Services.DbPOS>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(POSTWebApi.Services.DbPOS context)
        {
            #region custom Guid
            Guid[] guidFeatures = new Guid[7];
            guidFeatures[0] = new Guid("3cb39c12-3d6a-4f3e-a6cb-81073b2030c5");
            guidFeatures[1] = new Guid("dacdd0df-a5d5-47b4-9804-b3c678f09cd9");
            guidFeatures[2] = new Guid("7cd463e0-d531-4466-bd74-9370b666c016");
            guidFeatures[3] = new Guid("72f15699-a2cb-4eab-a9e1-017c5948ab40");
            guidFeatures[4] = new Guid("c7d78fbe-b993-43a1-bcc7-ae7bf0f9c8b0");
            guidFeatures[5] = new Guid("587278d3-3f9b-4661-81bb-9df37178f3bd");
            guidFeatures[6] = new Guid("34ec7e42-5619-4fb3-baac-98e68ffca1d9");

            Guid[] guidUsers = new Guid[1];
            guidUsers[0] = new Guid("e102467e-6afa-45df-99f0-4d5c2a862f02");

            Guid[] guidRoles = new Guid[1];
            guidRoles[0] = new Guid("f79cb651-dbba-4dfa-961b-c67034471f2c");
            #endregion

            #region generate data seed
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            context.Features.AddOrUpdate(x => x.Name,
                #region product
                new Feature() { Id = guidFeatures[0], Name = ConstantValue.FeatureValue.READ_PRODUCT_LIST },
                new Feature() { Id = guidFeatures[1], Name = ConstantValue.FeatureValue.READ_DELETED_PRODUCT_LIST },
                new Feature() { Id = guidFeatures[2], Name = ConstantValue.FeatureValue.READ_PRODUCT },
                new Feature() { Id = guidFeatures[3], Name = ConstantValue.FeatureValue.CREATE_PRODUCT },
                new Feature() { Id = guidFeatures[4], Name = ConstantValue.FeatureValue.EDIT_PRODUCT },
                new Feature() { Id = guidFeatures[5], Name = ConstantValue.FeatureValue.SOFT_DELETE_PRODUCT },
                new Feature() { Id = guidFeatures[6], Name = ConstantValue.FeatureValue.HARD_DELETE_PRODUCT }
                #endregion
            );

            context.Users.AddOrUpdate(x => x.Email, new User()
            {
                Id = guidUsers[0],
                Email = "su@super.com",
                Password = "123456",
                Name="Superuser",
                Gender = GenderEnum.Male
            });

            context.Roles.AddOrUpdate(x => x.Name, new Role() {
                Id = guidRoles[0],
                Name = "superuser"
            });

            context.UserRoles.AddOrUpdate(x => new {  x.UserId, x.RoleId }, new UserRole() {
                RoleId = guidRoles[0],
                UserId = guidUsers[0]
            });

            context.RoleFeatures.AddOrUpdate(x => new { x.RoleId, x.FeatureId },
                #region product
                new RoleFeature() { FeatureId = guidFeatures[0], RoleId = guidRoles[0] },
                new RoleFeature() { FeatureId = guidFeatures[1], RoleId = guidRoles[0] },
                new RoleFeature() { FeatureId = guidFeatures[2], RoleId = guidRoles[0] },
                new RoleFeature() { FeatureId = guidFeatures[3], RoleId = guidRoles[0] },
                new RoleFeature() { FeatureId = guidFeatures[4], RoleId = guidRoles[0] },
                new RoleFeature() { FeatureId = guidFeatures[5], RoleId = guidRoles[0] },
                new RoleFeature() { FeatureId = guidFeatures[6], RoleId = guidRoles[0] }
                #endregion
            );
            #endregion
        }
    }
}
