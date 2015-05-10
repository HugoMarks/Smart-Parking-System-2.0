namespace SPS.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inheritance1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Clients", new[] { "Parking_Id", "Parking_CNPJ" }, "dbo.Parkings");
            DropForeignKey("dbo.Collaborators", new[] { "Parking_Id", "Parking_CNPJ" }, "dbo.Parkings");
            DropForeignKey("dbo.ParkingSpaces", new[] { "Parking_Id", "Parking_CNPJ" }, "dbo.Parkings");
            DropForeignKey("dbo.Prices", new[] { "Parking_Id", "Parking_CNPJ" }, "dbo.Parkings");
            DropIndex("dbo.Parkings", new[] { "Id" });
            DropIndex("dbo.ParkingSpaces", new[] { "Parking_Id", "Parking_CNPJ" });
            DropIndex("dbo.Prices", new[] { "Parking_Id", "Parking_CNPJ" });
            DropIndex("dbo.Clients", new[] { "Parking_Id", "Parking_CNPJ" });
            DropIndex("dbo.Collaborators", new[] { "Parking_Id", "Parking_CNPJ" });
            DropColumn("dbo.Clients", "Parking_CNPJ");
            DropColumn("dbo.Collaborators", "Parking_CNPJ");
            DropColumn("dbo.ParkingSpaces", "Parking_CNPJ");
            DropColumn("dbo.Prices", "Parking_CNPJ");
            RenameColumn(table: "dbo.Clients", name: "Parking_Id", newName: "Parking_CNPJ");
            RenameColumn(table: "dbo.Collaborators", name: "Parking_Id", newName: "Parking_CNPJ");
            RenameColumn(table: "dbo.Parkings", name: "Id", newName: "LocalManager_Id");
            RenameColumn(table: "dbo.ParkingSpaces", name: "Parking_Id", newName: "Parking_CNPJ");
            RenameColumn(table: "dbo.Prices", name: "Parking_Id", newName: "Parking_CNPJ");
            DropPrimaryKey("dbo.Parkings");
            AlterColumn("dbo.Clients", "Parking_CNPJ", c => c.String(maxLength: 128));
            AlterColumn("dbo.Collaborators", "Parking_CNPJ", c => c.String(maxLength: 128));
            AlterColumn("dbo.Parkings", "LocalManager_Id", c => c.Int());
            AlterColumn("dbo.ParkingSpaces", "Parking_CNPJ", c => c.String(maxLength: 128));
            AlterColumn("dbo.Prices", "Parking_CNPJ", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.Parkings", "CNPJ");
            CreateIndex("dbo.Parkings", "LocalManager_Id");
            CreateIndex("dbo.ParkingSpaces", "Parking_CNPJ");
            CreateIndex("dbo.Prices", "Parking_CNPJ");
            CreateIndex("dbo.Clients", "Parking_CNPJ");
            CreateIndex("dbo.Collaborators", "Parking_CNPJ");
            AddForeignKey("dbo.Clients", "Parking_CNPJ", "dbo.Parkings", "CNPJ");
            AddForeignKey("dbo.Collaborators", "Parking_CNPJ", "dbo.Parkings", "CNPJ");
            AddForeignKey("dbo.ParkingSpaces", "Parking_CNPJ", "dbo.Parkings", "CNPJ");
            AddForeignKey("dbo.Prices", "Parking_CNPJ", "dbo.Parkings", "CNPJ");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Prices", "Parking_CNPJ", "dbo.Parkings");
            DropForeignKey("dbo.ParkingSpaces", "Parking_CNPJ", "dbo.Parkings");
            DropForeignKey("dbo.Collaborators", "Parking_CNPJ", "dbo.Parkings");
            DropForeignKey("dbo.Clients", "Parking_CNPJ", "dbo.Parkings");
            DropIndex("dbo.Collaborators", new[] { "Parking_CNPJ" });
            DropIndex("dbo.Clients", new[] { "Parking_CNPJ" });
            DropIndex("dbo.Prices", new[] { "Parking_CNPJ" });
            DropIndex("dbo.ParkingSpaces", new[] { "Parking_CNPJ" });
            DropIndex("dbo.Parkings", new[] { "LocalManager_Id" });
            DropPrimaryKey("dbo.Parkings");
            AlterColumn("dbo.Prices", "Parking_CNPJ", c => c.Int());
            AlterColumn("dbo.ParkingSpaces", "Parking_CNPJ", c => c.Int());
            AlterColumn("dbo.Parkings", "LocalManager_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Collaborators", "Parking_CNPJ", c => c.Int());
            AlterColumn("dbo.Clients", "Parking_CNPJ", c => c.Int());
            AddPrimaryKey("dbo.Parkings", new[] { "Id", "CNPJ" });
            RenameColumn(table: "dbo.Prices", name: "Parking_CNPJ", newName: "Parking_Id");
            RenameColumn(table: "dbo.ParkingSpaces", name: "Parking_CNPJ", newName: "Parking_Id");
            RenameColumn(table: "dbo.Parkings", name: "LocalManager_Id", newName: "Id");
            RenameColumn(table: "dbo.Collaborators", name: "Parking_CNPJ", newName: "Parking_Id");
            RenameColumn(table: "dbo.Clients", name: "Parking_CNPJ", newName: "Parking_Id");
            AddColumn("dbo.Prices", "Parking_CNPJ", c => c.String(maxLength: 128));
            AddColumn("dbo.ParkingSpaces", "Parking_CNPJ", c => c.String(maxLength: 128));
            AddColumn("dbo.Collaborators", "Parking_CNPJ", c => c.String(maxLength: 128));
            AddColumn("dbo.Clients", "Parking_CNPJ", c => c.String(maxLength: 128));
            CreateIndex("dbo.Collaborators", new[] { "Parking_Id", "Parking_CNPJ" });
            CreateIndex("dbo.Clients", new[] { "Parking_Id", "Parking_CNPJ" });
            CreateIndex("dbo.Prices", new[] { "Parking_Id", "Parking_CNPJ" });
            CreateIndex("dbo.ParkingSpaces", new[] { "Parking_Id", "Parking_CNPJ" });
            CreateIndex("dbo.Parkings", "Id");
            AddForeignKey("dbo.Prices", new[] { "Parking_Id", "Parking_CNPJ" }, "dbo.Parkings", new[] { "Id", "CNPJ" });
            AddForeignKey("dbo.ParkingSpaces", new[] { "Parking_Id", "Parking_CNPJ" }, "dbo.Parkings", new[] { "Id", "CNPJ" });
            AddForeignKey("dbo.Collaborators", new[] { "Parking_Id", "Parking_CNPJ" }, "dbo.Parkings", new[] { "Id", "CNPJ" });
            AddForeignKey("dbo.Clients", new[] { "Parking_Id", "Parking_CNPJ" }, "dbo.Parkings", new[] { "Id", "CNPJ" });
        }
    }
}
