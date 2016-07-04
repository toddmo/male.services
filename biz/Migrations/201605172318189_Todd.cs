namespace male.services.biz.Migrations
{
  using System.Data.Entity.Migrations;

  public partial class Todd : DbMigration
    {
        public override void Up()
        {

            
            CreateTable(
                "dbo.Fulfillments",
                c => new
                    {
                        Member_Id = c.Int(nullable: false),
                        Package_Id = c.Int(nullable: false),
                        Promise_Id = c.Int(nullable: false),
                        Id = c.Int(nullable: false, identity: true),
                        Purchase_Id = c.Int(nullable: false),
                        Date = c.DateTime(),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Member_Id, t.Package_Id, t.Promise_Id, t.Id })
                .ForeignKey("dbo.Promises", t => new { t.Member_Id, t.Package_Id, t.Promise_Id })
                .ForeignKey("dbo.Purchases", t => new { t.Member_Id, t.Package_Id, t.Purchase_Id })
                .Index(t => new { t.Member_Id, t.Package_Id, t.Promise_Id })
                .Index(t => new { t.Member_Id, t.Package_Id, t.Purchase_Id });
            
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Fulfillments", new[] { "Member_Id", "Package_Id", "Purchase_Id" }, "dbo.Purchases");
            DropForeignKey("dbo.Fulfillments", new[] { "Member_Id", "Package_Id", "Promise_Id" }, "dbo.Promises");
            DropIndex("dbo.Fulfillments", new[] { "Member_Id", "Package_Id", "Purchase_Id" });
            DropIndex("dbo.Fulfillments", new[] { "Member_Id", "Package_Id", "Promise_Id" });
            DropTable("dbo.Fulfillments");
        }
    }
}
