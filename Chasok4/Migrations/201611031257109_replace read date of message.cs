namespace Chasok4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class replacereaddateofmessage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserMessages", "ReadDate", c => c.DateTime());
            DropColumn("dbo.Messages", "ReadDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Messages", "ReadDate", c => c.DateTime());
            DropColumn("dbo.UserMessages", "ReadDate");
        }
    }
}
