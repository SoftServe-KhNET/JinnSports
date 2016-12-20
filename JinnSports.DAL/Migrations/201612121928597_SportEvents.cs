using System;
using System.Data.Entity.Migrations;

namespace JinnSports.DAL.Migrations
{
    public partial class SportEvents : DbMigration
    {
        public override void Up()
        {
            this.RenameTable(name: "dbo.CompetitionEvents", newName: "SportEvents");
            this.RenameColumn(table: "dbo.Results", name: "CompetitionEvent_Id", newName: "SportEvent_Id");
            this.RenameIndex(table: "dbo.Results", name: "IX_CompetitionEvent_Id", newName: "IX_SportEvent_Id");
            this.AddColumn("dbo.SportEvents", "SportType_Id", c => c.Int());
            this.AlterColumn("dbo.Results", "Score", c => c.Int(nullable: false));
            this.CreateIndex("dbo.SportEvents", "SportType_Id");
            this.AddForeignKey("dbo.SportEvents", "SportType_Id", "dbo.SportTypes", "Id");
            this.Sql(@"INSERT INTO [dbo].[SportTypes] ([NAME]) VALUES('Football')
                  INSERT INTO [dbo].[SportTypes] ([NAME]) VALUES('Basketball')
                  INSERT INTO [dbo].[SportTypes] ([NAME]) VALUES('Hockey')");
        }
        
        public override void Down()
        {
            this.DropForeignKey("dbo.SportEvents", "SportType_Id", "dbo.SportTypes");
            this.DropIndex("dbo.SportEvents", new[] { "SportType_Id" });
            this.AlterColumn("dbo.Results", "Score", c => c.String());
            this.DropColumn("dbo.SportEvents", "SportType_Id");
            this.RenameIndex(table: "dbo.Results", name: "IX_SportEvent_Id", newName: "IX_CompetitionEvent_Id");
            this.RenameColumn(table: "dbo.Results", name: "SportEvent_Id", newName: "CompetitionEvent_Id");
            this.RenameTable(name: "dbo.SportEvents", newName: "CompetitionEvents");
        }
    }
}
