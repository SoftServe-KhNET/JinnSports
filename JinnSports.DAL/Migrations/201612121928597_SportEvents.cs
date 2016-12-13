using System;
using System.Data.Entity.Migrations;

namespace JinnSports.DAL.Migrations
{
    public partial class SportEvents : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CompetitionEvents", newName: "SportEvents");
            RenameColumn(table: "dbo.Results", name: "CompetitionEvent_Id", newName: "SportEvent_Id");
            RenameIndex(table: "dbo.Results", name: "IX_CompetitionEvent_Id", newName: "IX_SportEvent_Id");
            AddColumn("dbo.SportEvents", "SportType_Id", c => c.Int());
            AlterColumn("dbo.Results", "Score", c => c.Int(nullable: false));
            CreateIndex("dbo.SportEvents", "SportType_Id");
            AddForeignKey("dbo.SportEvents", "SportType_Id", "dbo.SportTypes", "Id");
            Sql(@"INSERT INTO [dbo].[SportTypes] ([NAME]) VALUES('Football')
                  INSERT INTO [dbo].[SportTypes] ([NAME]) VALUES('Basletball')
                  INSERT INTO [dbo].[SportTypes] ([NAME]) VALUES('Hockey')");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SportEvents", "SportType_Id", "dbo.SportTypes");
            DropIndex("dbo.SportEvents", new[] { "SportType_Id" });
            AlterColumn("dbo.Results", "Score", c => c.String());
            DropColumn("dbo.SportEvents", "SportType_Id");
            RenameIndex(table: "dbo.Results", name: "IX_SportEvent_Id", newName: "IX_CompetitionEvent_Id");
            RenameColumn(table: "dbo.Results", name: "SportEvent_Id", newName: "CompetitionEvent_Id");
            RenameTable(name: "dbo.SportEvents", newName: "CompetitionEvents");
        }
    }
}
