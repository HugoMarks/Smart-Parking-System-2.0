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
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address_PostalCode = c.String(maxLength: 128),
                        LocalManager_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
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
                        Parking_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Number)
                .ForeignKey("dbo.Parkings", t => t.Parking_Id)
                .Index(t => t.Parking_Id);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Client_Id = c.Int(),
                        IUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.Client_Id)
                .ForeignKey("dbo.Users", t => t.IUser_Id)
                .Index(t => t.Client_Id)
                .Index(t => t.IUser_Id);
            
            CreateTable(
                "dbo.Prices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Parking_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Parkings", t => t.Parking_Id)
                .Index(t => t.Parking_Id);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Parking_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Id)
                .ForeignKey("dbo.Parkings", t => t.Parking_Id)
                .Index(t => t.Id)
                .Index(t => t.Parking_Id);
            
            CreateTable(
                "dbo.Collaborators",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Parking_Id = c.Int(),
                        Salary = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Id)
                .ForeignKey("dbo.Parkings", t => t.Parking_Id)
                .Index(t => t.Id)
                .Index(t => t.Parking_Id);
            
            CreateTable(
                "dbo.GlobalManagers",
                c => new
                    {
                        Id = c.Int(nullable: false),
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
            DropForeignKey("dbo.Collaborators", "Parking_Id", "dbo.Parkings");
            DropForeignKey("dbo.Collaborators", "Id", "dbo.Users");
            DropForeignKey("dbo.Clients", "Parking_Id", "dbo.Parkings");
            DropForeignKey("dbo.Clients", "Id", "dbo.Users");
            DropForeignKey("dbo.Prices", "Parking_Id", "dbo.Parkings");
            DropForeignKey("dbo.Tags", "IUser_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "Address_PostalCode", "dbo.Addresses");
            DropForeignKey("dbo.Tags", "Client_Id", "dbo.Clients");
            DropForeignKey("dbo.ParkingSpaces", "Parking_Id", "dbo.Parkings");
            DropForeignKey("dbo.Parkings", "LocalManager_Id", "dbo.LocalManagers");
            DropForeignKey("dbo.Parkings", "Address_PostalCode", "dbo.Addresses");
            DropIndex("dbo.LocalManagers", new[] { "Id" });
            DropIndex("dbo.GlobalManagers", new[] { "Id" });
            DropIndex("dbo.Collaborators", new[] { "Parking_Id" });
            DropIndex("dbo.Collaborators", new[] { "Id" });
            DropIndex("dbo.Clients", new[] { "Parking_Id" });
            DropIndex("dbo.Clients", new[] { "Id" });
            DropIndex("dbo.Prices", new[] { "Parking_Id" });
            DropIndex("dbo.Tags", new[] { "IUser_Id" });
            DropIndex("dbo.Tags", new[] { "Client_Id" });
            DropIndex("dbo.ParkingSpaces", new[] { "Parking_Id" });
            DropIndex("dbo.Parkings", new[] { "LocalManager_Id" });
            DropIndex("dbo.Parkings", new[] { "Address_PostalCode" });
            DropIndex("dbo.Users", new[] { "Address_PostalCode" });
            DropTable("dbo.LocalManagers");
            DropTable("dbo.GlobalManagers");
            DropTable("dbo.Collaborators");
            DropTable("dbo.Clients");
            DropTable("dbo.Prices");
            DropTable("dbo.Tags");
            DropTable("dbo.ParkingSpaces");
            DropTable("dbo.Parkings");
            DropTable("dbo.Users");
            DropTable("dbo.Addresses");
        }
    }
}
