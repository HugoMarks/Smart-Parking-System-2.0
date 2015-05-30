namespace SPS.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inheritance1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UsageRecords", "IsDirty", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UsageRecords", "IsDirty");
        }
    }
}
