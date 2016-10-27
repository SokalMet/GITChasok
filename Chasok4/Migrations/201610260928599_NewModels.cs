namespace Chasok4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewModels : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserMessages", "UserReceiveId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserMessages", "UserSendId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserMessages", "MessageId", "dbo.Messages");
            DropIndex("dbo.UserMessages", new[] { "MessageId" });
            DropIndex("dbo.UserMessages", new[] { "UserSendId" });
            DropIndex("dbo.UserMessages", new[] { "UserReceiveId" });
            AddColumn("dbo.Messages", "CreateData", c => c.DateTime(nullable: false));
            AddColumn("dbo.Messages", "ReadData", c => c.DateTime());
            AddColumn("dbo.Messages", "ReadStatus", c => c.Boolean(nullable: false));
            AddColumn("dbo.Messages", "AppUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Messages", "Creator_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "MessageToSend_Id", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Message_Id", c => c.Int());
            CreateIndex("dbo.Messages", "AppUser_Id");
            CreateIndex("dbo.Messages", "Creator_Id");
            CreateIndex("dbo.AspNetUsers", "MessageToSend_Id");
            CreateIndex("dbo.AspNetUsers", "Message_Id");
            AddForeignKey("dbo.Messages", "AppUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "MessageToSend_Id", "dbo.Messages", "Id");
            AddForeignKey("dbo.AspNetUsers", "Message_Id", "dbo.Messages", "Id");
            AddForeignKey("dbo.Messages", "Creator_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Messages", "RoomName");
            DropTable("dbo.ConversationRooms");
            DropTable("dbo.UserMessages");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserMessages",
                c => new
                    {
                        UserMessageId = c.Int(nullable: false, identity: true),
                        DataTimeSend = c.DateTime(nullable: false),
                        DataTimeRead = c.DateTime(nullable: false),
                        ReadStatus = c.Boolean(nullable: false),
                        MessageId = c.Int(nullable: false),
                        UserSendId = c.String(maxLength: 128),
                        UserReceiveId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserMessageId);
            
            CreateTable(
                "dbo.ConversationRooms",
                c => new
                    {
                        RoomName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.RoomName);
            
            AddColumn("dbo.Messages", "RoomName", c => c.String());
            DropForeignKey("dbo.Messages", "Creator_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Message_Id", "dbo.Messages");
            DropForeignKey("dbo.AspNetUsers", "MessageToSend_Id", "dbo.Messages");
            DropForeignKey("dbo.Messages", "AppUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "Message_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "MessageToSend_Id" });
            DropIndex("dbo.Messages", new[] { "Creator_Id" });
            DropIndex("dbo.Messages", new[] { "AppUser_Id" });
            DropColumn("dbo.AspNetUsers", "Message_Id");
            DropColumn("dbo.AspNetUsers", "MessageToSend_Id");
            DropColumn("dbo.Messages", "Creator_Id");
            DropColumn("dbo.Messages", "AppUser_Id");
            DropColumn("dbo.Messages", "ReadStatus");
            DropColumn("dbo.Messages", "ReadData");
            DropColumn("dbo.Messages", "CreateData");
            CreateIndex("dbo.UserMessages", "UserReceiveId");
            CreateIndex("dbo.UserMessages", "UserSendId");
            CreateIndex("dbo.UserMessages", "MessageId");
            AddForeignKey("dbo.UserMessages", "MessageId", "dbo.Messages", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserMessages", "UserSendId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.UserMessages", "UserReceiveId", "dbo.AspNetUsers", "Id");
        }
    }
}
