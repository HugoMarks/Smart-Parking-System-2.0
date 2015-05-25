namespace SPS.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inheritance1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.UsageRecords", name: "MonthlyClient_Id", newName: "Client_Id");
            RenameIndex(table: "dbo.UsageRecords", name: "IX_MonthlyClient_Id", newName: "IX_Client_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.UsageRecords", name: "IX_Client_Id", newName: "IX_MonthlyClient_Id");
            RenameColumn(table: "dbo.UsageRecords", name: "Client_Id", newName: "MonthlyClient_Id");
        }
    }
}
