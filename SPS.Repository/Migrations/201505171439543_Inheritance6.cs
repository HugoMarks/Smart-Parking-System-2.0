namespace SPS.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inheritance6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "StreetNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "StreetNumber");
        }
    }
}
