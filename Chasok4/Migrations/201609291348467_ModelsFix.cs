namespace Chasok4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelsFix : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Messages", "CreateDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Messages", "CreateDate", c => c.DateTime(nullable: false));
        }
    }
}
