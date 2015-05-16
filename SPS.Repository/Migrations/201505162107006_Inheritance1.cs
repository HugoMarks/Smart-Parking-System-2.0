namespace SPS.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inheritance1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GlobalManagers", "TokenHash", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GlobalManagers", "TokenHash");
        }
    }
}
