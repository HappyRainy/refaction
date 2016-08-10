namespace Xero.RefactoringExercise.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProduct : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Product",
                c => new
                {
                    Id = c.Guid(nullable: false, identity: true),
                    CreatedOn = c.DateTime(nullable: false, defaultValue: DateTime.UtcNow),
                    UpdatedOn = c.DateTime(nullable: true),
                    CreatedBy = c.String(nullable: false, maxLength:128),
                    UpdatedBy = c.String(nullable: true, maxLength: 128),
                    RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    Name = c.String(nullable: false,  maxLength: 100),
                    Description = c.String(nullable: true, maxLength: 500),
                    Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    DeliveryPrice = c.Decimal(nullable: false, precision: 18, scale: 2)
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Product", new[] { "Name" });
            DropTable("dbo.Product");
        }
    }
}
