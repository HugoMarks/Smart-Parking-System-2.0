namespace SPS.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inheritance : DbMigration
    {
        public override void Up()
        {            
            CreateTable(
                "dbo.ParkingSpaces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Parking_CNPJ = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Parkings", t => t.Parking_CNPJ)
                .Index(t => t.Parking_CNPJ);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ParkingSpaces", "Parking_CNPJ", "dbo.Parkings");
            DropIndex("dbo.ParkingSpaces", new[] { "Parking_CNPJ" });
            DropTable("dbo.ParkingSpaces");
        }
    }
}
