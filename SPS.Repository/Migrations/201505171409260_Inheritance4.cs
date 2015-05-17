namespace SPS.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inheritance4 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Tags", name: "Client_Id", newName: "MonthlyClient_Id");
            RenameColumn(table: "dbo.Tags", name: "IUser_Id", newName: "User_Id");
            RenameIndex(table: "dbo.Tags", name: "IX_IUser_Id", newName: "IX_User_Id");
            RenameIndex(table: "dbo.Tags", name: "IX_Client_Id", newName: "IX_MonthlyClient_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Tags", name: "IX_MonthlyClient_Id", newName: "IX_Client_Id");
            RenameIndex(table: "dbo.Tags", name: "IX_User_Id", newName: "IX_IUser_Id");
            RenameColumn(table: "dbo.Tags", name: "User_Id", newName: "IUser_Id");
            RenameColumn(table: "dbo.Tags", name: "MonthlyClient_Id", newName: "Client_Id");
        }
    }
}
