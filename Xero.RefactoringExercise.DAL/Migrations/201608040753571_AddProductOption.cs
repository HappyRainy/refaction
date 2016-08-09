namespace Xero.RefactoringExercise.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductOption : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductOption",
                c => new
                {
                    Id = c.Guid(nullable: false, identity: true),
                    CreatedOn = c.DateTime(nullable: false, defaultValue: DateTime.UtcNow),
                    UpdatedOn = c.DateTime(nullable: true),
                    CreatedBy = c.String(nullable: false, maxLength: 128),
                    UpdatedBy = c.String(nullable: false, maxLength: 128),
                    RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    ProductId = c.Guid(nullable: false),
                    Name = c.String(nullable: false, maxLength: 100),
                    Description = c.String(nullable: true, maxLength: 500)
                })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.ProductId, t.Name}, unique: true);

            AddForeignKey("dbo.ProductOption", "ProductId", "dbo.Product", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductOption", "ProductId", "dbo.Product");
            DropTable("dbo.ProductOption");
        }
    }
}
