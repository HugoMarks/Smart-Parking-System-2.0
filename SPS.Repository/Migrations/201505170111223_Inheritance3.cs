namespace SPS.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inheritance3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Complement", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Complement");
        }
    }
}
