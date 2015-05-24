namespace SPS.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inheritance : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Prices", "Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Prices", new[] { "Id" });
        }
    }
}
