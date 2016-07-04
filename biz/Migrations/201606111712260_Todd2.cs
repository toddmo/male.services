namespace male.services.biz.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Todd2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "Image", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "Image");
        }
    }
}
