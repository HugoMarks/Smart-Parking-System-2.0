namespace SPS.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inheritance : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        PostalCode = c.String(nullable: false, maxLength: 128),
                        Street = c.String(),
                        City = c.String(),
                        Square = c.String(),
                        State = c.String(),
                    })
                .PrimaryKey(t => t.PostalCode);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Telephone = c.String(),
                        StreetNumber = c.Int(nullable: false),
                        Complement = c.String(),
                        RG = c.String(),
                        CPF = c.String(),
                        Password = c.String(),
                        Address_PostalCode = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.Address_PostalCode)
                .Index(t => t.Address_PostalCode);
            
            CreateTable(
                "dbo.Parkings",
                c => new
                    {
                        CNPJ = c.String(nullable: false, maxLength: 128),
                        PhoneNumber = c.String(),
                        Name = c.String(),
                        Address_PostalCode = c.String(maxLength: 128),
                        LocalManager_Id = c.Int(),
                    })
                .PrimaryKey(t => t.CNPJ)
                .ForeignKey("dbo.Addresses", t => t.Address_PostalCode)
                .ForeignKey("dbo.LocalManagers", t => t.LocalManager_Id)
                .Index(t => t.Address_PostalCode)
                .Index(t => t.LocalManager_Id);
            
            CreateTable(
                "dbo.ParkingSpaces",
                c => new
                    {
                        Number = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        Parking_CNPJ = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Number)
                .ForeignKey("dbo.Parkings", t => t.Parking_CNPJ)
                .Index(t => t.Parking_CNPJ);
            
            CreateTable(
                "dbo.UsageRecords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EnterDateTime = c.DateTime(nullable: false),
                        ExitDateTime = c.DateTime(nullable: false),
                        TotalHours = c.Long(nullable: false),
                        TotalCash = c.Decimal(nullable: false, precision: 18, scale: 2),
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
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        User_Id = c.Int(),
                        MonthlyClient_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .ForeignKey("dbo.Clients", t => t.MonthlyClient_Id)
                .Index(t => t.User_Id)
                .Index(t => t.MonthlyClient_Id);
            
            CreateTable(
                "dbo.Prices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Parking_CNPJ = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Parkings", t => t.Parking_CNPJ)
                .Index(t => t.Parking_CNPJ);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Parking_CNPJ = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Id)
                .ForeignKey("dbo.Parkings", t => t.Parking_CNPJ)
                .Index(t => t.Id)
                .Index(t => t.Parking_CNPJ);
            
            CreateTable(
                "dbo.Collaborators",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Parking_CNPJ = c.String(maxLength: 128),
                        Salary = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Id)
                .ForeignKey("dbo.Parkings", t => t.Parking_CNPJ)
                .Index(t => t.Id)
                .Index(t => t.Parking_CNPJ);
            
            CreateTable(
                "dbo.GlobalManagers",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        TokenHash = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.LocalManagers",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LocalManagers", "Id", "dbo.Users");
            DropForeignKey("dbo.GlobalManagers", "Id", "dbo.Users");
            DropForeignKey("dbo.Collaborators", "Parking_CNPJ", "dbo.Parkings");
            DropForeignKey("dbo.Collaborators", "Id", "dbo.Users");
            DropForeignKey("dbo.Clients", "Parking_CNPJ", "dbo.Parkings");
            DropForeignKey("dbo.Clients", "Id", "dbo.Users");
            DropForeignKey("dbo.Prices", "Parking_CNPJ", "dbo.Parkings");
            DropForeignKey("dbo.Tags", "MonthlyClient_Id", "dbo.Clients");
            DropForeignKey("dbo.UsageRecords", "MonthlyClient_Id", "dbo.Clients");
            DropForeignKey("dbo.UsageRecords", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.Tags", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "Address_PostalCode", "dbo.Addresses");
            DropForeignKey("dbo.UsageRecords", "Parking_CNPJ", "dbo.Parkings");
            DropForeignKey("dbo.ParkingSpaces", "Parking_CNPJ", "dbo.Parkings");
            DropForeignKey("dbo.Parkings", "LocalManager_Id", "dbo.LocalManagers");
            DropForeignKey("dbo.Parkings", "Address_PostalCode", "dbo.Addresses");
            DropIndex("dbo.LocalManagers", new[] { "Id" });
            DropIndex("dbo.GlobalManagers", new[] { "Id" });
            DropIndex("dbo.Collaborators", new[] { "Parking_CNPJ" });
            DropIndex("dbo.Collaborators", new[] { "Id" });
            DropIndex("dbo.Clients", new[] { "Parking_CNPJ" });
            DropIndex("dbo.Clients", new[] { "Id" });
            DropIndex("dbo.Prices", new[] { "Parking_CNPJ" });
            DropIndex("dbo.Tags", new[] { "MonthlyClient_Id" });
            DropIndex("dbo.Tags", new[] { "User_Id" });
            DropIndex("dbo.UsageRecords", new[] { "MonthlyClient_Id" });
            DropIndex("dbo.UsageRecords", new[] { "Tag_Id" });
            DropIndex("dbo.UsageRecords", new[] { "Parking_CNPJ" });
            DropIndex("dbo.ParkingSpaces", new[] { "Parking_CNPJ" });
            DropIndex("dbo.Parkings", new[] { "LocalManager_Id" });
            DropIndex("dbo.Parkings", new[] { "Address_PostalCode" });
            DropIndex("dbo.Users", new[] { "Address_PostalCode" });
            DropTable("dbo.LocalManagers");
            DropTable("dbo.GlobalManagers");
            DropTable("dbo.Collaborators");
            DropTable("dbo.Clients");
            DropTable("dbo.Prices");
            DropTable("dbo.Tags");
            DropTable("dbo.UsageRecords");
            DropTable("dbo.ParkingSpaces");
            DropTable("dbo.Parkings");
            DropTable("dbo.Users");
            DropTable("dbo.Addresses");
        }
    }
}
