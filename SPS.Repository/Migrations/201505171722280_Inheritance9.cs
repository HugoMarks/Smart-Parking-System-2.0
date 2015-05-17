namespace SPS.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inheritance9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UsageRecords", "TotalHours", c => c.Long(nullable: false));
            AddColumn("dbo.UsageRecords", "TotalCash", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UsageRecords", "TotalCash");
            DropColumn("dbo.UsageRecords", "TotalHours");
        }
    }
}
