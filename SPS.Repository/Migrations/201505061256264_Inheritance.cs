namespace SPS.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inheritance : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Addresses", "LocalManager_Id", "dbo.LocalManagers");
            DropIndex("dbo.Addresses", new[] { "LocalManager_Id" });
            AddColumn("dbo.Parkings", "LocalManager_Id", c => c.Int());
            CreateIndex("dbo.Parkings", "LocalManager_Id");
            AddForeignKey("dbo.Parkings", "LocalManager_Id", "dbo.LocalManagers", "Id");
            DropColumn("dbo.Addresses", "LocalManager_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Addresses", "LocalManager_Id", c => c.Int());
            DropForeignKey("dbo.Parkings", "LocalManager_Id", "dbo.LocalManagers");
            DropIndex("dbo.Parkings", new[] { "LocalManager_Id" });
            DropColumn("dbo.Parkings", "LocalManager_Id");
            CreateIndex("dbo.Addresses", "LocalManager_Id");
            AddForeignKey("dbo.Addresses", "LocalManager_Id", "dbo.LocalManagers", "Id");
        }
    }
}
