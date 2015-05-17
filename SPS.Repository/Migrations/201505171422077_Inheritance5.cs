namespace SPS.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inheritance5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "StreetNumber", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "StreetNumber");
        }
    }
}
