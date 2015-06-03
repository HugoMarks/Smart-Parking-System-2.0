namespace SPS.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inheritance2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prices", "IsDefault", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Prices", "IsDefault");
        }
    }
}
