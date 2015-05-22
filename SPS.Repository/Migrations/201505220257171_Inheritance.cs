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
                "dbo.Clients",
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
                        Parking_CNPJ = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.Address_PostalCode)
                .ForeignKey("dbo.Parkings", t => t.Parking_CNPJ)
                .Index(t => t.Address_PostalCode)
                .Index(t => t.Parking_CNPJ);
            
            CreateTable(
                "dbo.Parkings",
                c => new
                    {
                        CNPJ = c.String(nullable: false, maxLength: 128),
                        PhoneNumber = c.String(),
                        Name = c.String(),
                        StreetNumber = c.Int(nullable: false),
                        Address_PostalCode = c.String(maxLength: 128),
                        LocalManager_Id = c.Int(),
                    })
                .PrimaryKey(t => t.CNPJ)
                .ForeignKey("dbo.Addresses", t => t.Address_PostalCode)
                .ForeignKey("dbo.LocalManagers", t => t.LocalManager_Id)
                .Index(t => t.Address_PostalCode)
                .Index(t => t.LocalManager_Id);
            
            CreateTable(
                "dbo.Collaborators",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Salary = c.Decimal(nullable: false, precision: 18, scale: 2),
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
                        Parking_CNPJ = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.Address_PostalCode)
                .ForeignKey("dbo.Parkings", t => t.Parking_CNPJ)
                .Index(t => t.Address_PostalCode)
                .Index(t => t.Parking_CNPJ);
            
            CreateTable(
                "dbo.LocalManagers",
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
                        TotalHours = c.Single(nullable: false),
                        TotalCash = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentStatus = c.Int(nullable: false),
                        Parking_CNPJ = c.String(maxLength: 128),
                        Tag_Id = c.String(maxLength: 128),
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
                        Id = c.String(nullable: false, maxLength: 128),
                        SpaceNumber = c.Int(nullable: false),
                        Client_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.Client_Id)
                .Index(t => t.Client_Id);
            
            CreateTable(
                "dbo.GlobalManagers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TokenHash = c.String(),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Prices", "Parking_CNPJ", "dbo.Parkings");
            DropForeignKey("dbo.GlobalManagers", "Address_PostalCode", "dbo.Addresses");
            DropForeignKey("dbo.UsageRecords", "MonthlyClient_Id", "dbo.Clients");
            DropForeignKey("dbo.UsageRecords", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.Tags", "Client_Id", "dbo.Clients");
            DropForeignKey("dbo.UsageRecords", "Parking_CNPJ", "dbo.Parkings");
            DropForeignKey("dbo.Clients", "Parking_CNPJ", "dbo.Parkings");
            DropForeignKey("dbo.ParkingSpaces", "Parking_CNPJ", "dbo.Parkings");
            DropForeignKey("dbo.Parkings", "LocalManager_Id", "dbo.LocalManagers");
            DropForeignKey("dbo.LocalManagers", "Address_PostalCode", "dbo.Addresses");
            DropForeignKey("dbo.Collaborators", "Parking_CNPJ", "dbo.Parkings");
            DropForeignKey("dbo.Collaborators", "Address_PostalCode", "dbo.Addresses");
            DropForeignKey("dbo.Parkings", "Address_PostalCode", "dbo.Addresses");
            DropForeignKey("dbo.Clients", "Address_PostalCode", "dbo.Addresses");
            DropIndex("dbo.Prices", new[] { "Parking_CNPJ" });
            DropIndex("dbo.GlobalManagers", new[] { "Address_PostalCode" });
            DropIndex("dbo.Tags", new[] { "Client_Id" });
            DropIndex("dbo.UsageRecords", new[] { "MonthlyClient_Id" });
            DropIndex("dbo.UsageRecords", new[] { "Tag_Id" });
            DropIndex("dbo.UsageRecords", new[] { "Parking_CNPJ" });
            DropIndex("dbo.ParkingSpaces", new[] { "Parking_CNPJ" });
            DropIndex("dbo.LocalManagers", new[] { "Address_PostalCode" });
            DropIndex("dbo.Collaborators", new[] { "Parking_CNPJ" });
            DropIndex("dbo.Collaborators", new[] { "Address_PostalCode" });
            DropIndex("dbo.Parkings", new[] { "LocalManager_Id" });
            DropIndex("dbo.Parkings", new[] { "Address_PostalCode" });
            DropIndex("dbo.Clients", new[] { "Parking_CNPJ" });
            DropIndex("dbo.Clients", new[] { "Address_PostalCode" });
            DropTable("dbo.Prices");
            DropTable("dbo.GlobalManagers");
            DropTable("dbo.Tags");
            DropTable("dbo.UsageRecords");
            DropTable("dbo.ParkingSpaces");
            DropTable("dbo.LocalManagers");
            DropTable("dbo.Collaborators");
            DropTable("dbo.Parkings");
            DropTable("dbo.Clients");
            DropTable("dbo.Addresses");
        }
    }
}
