namespace SPS.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inheritance8 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UsageRecords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EnterDateTime = c.DateTime(nullable: false),
                        ExitDateTime = c.DateTime(nullable: false),
                        Parking_CNPJ = c.String(maxLength: 128),
                        Tag_Id = c.Int(),
                        MonthlyClient_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Parkings", t => t.Parking_CNPJ)
                .ForeignKey("dbo.Tags", t => t.Tag_Id)
                .ForeignKey("dbo.Clients", t => t.MonthlyClient_Id)
                .Index(t => t.Parking_CNPJ)
                .Index(t => t.Tag_Id)
                .Index(t => t.MonthlyClient_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsageRecords", "MonthlyClient_Id", "dbo.Clients");
            DropForeignKey("dbo.UsageRecords", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.UsageRecords", "Parking_CNPJ", "dbo.Parkings");
            DropIndex("dbo.UsageRecords", new[] { "MonthlyClient_Id" });
            DropIndex("dbo.UsageRecords", new[] { "Tag_Id" });
            DropIndex("dbo.UsageRecords", new[] { "Parking_CNPJ" });
            DropTable("dbo.UsageRecords");
        }
    }
}
