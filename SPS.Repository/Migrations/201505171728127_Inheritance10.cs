namespace SPS.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inheritance10 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UsageRecords", "TotalHours", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UsageRecords", "TotalHours", c => c.Long(nullable: false));
        }
    }
}
