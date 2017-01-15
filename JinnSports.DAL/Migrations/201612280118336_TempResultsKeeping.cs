namespace JinnSports.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TempResultsKeeping : DbMigration
    {
        public override void Up()
        {
            this.DropForeignKey("dbo.Conformities", "ExistedTeam_Id", "dbo.Teams");
            this.DropIndex("dbo.Conformities", new[] { "ExistedTeam_Id" });
            this.CreateTable(
                "dbo.TempResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Score = c.Int(),
                        IsHome = c.Boolean(nullable: false),
                        Team_Id = c.Int(),
                        TempSportEvent_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.Team_Id)
                .ForeignKey("dbo.TempSportEvents", t => t.TempSportEvent_Id)
                .Index(t => t.Team_Id)
                .Index(t => t.TempSportEvent_Id);

            this.CreateTable(
                "dbo.TempSportEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        SportType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SportTypes", t => t.SportType_Id)
                .Index(t => t.SportType_Id);

            this.AddColumn("dbo.Conformities", "InputName", c => c.String());
            this.AddColumn("dbo.Conformities", "ExistedName", c => c.String());
            this.AddColumn("dbo.Conformities", "TempResult_Id", c => c.Int());
            this.CreateIndex("dbo.Conformities", "TempResult_Id");
            this.AddForeignKey("dbo.Conformities", "TempResult_Id", "dbo.TempResults", "Id");
            this.DropColumn("dbo.Conformities", "InputTeamName");
            this.DropColumn("dbo.Conformities", "ExistedTeam_Id");
        }
        
        public override void Down()
        {
            this.AddColumn("dbo.Conformities", "ExistedTeam_Id", c => c.Int());
            this.AddColumn("dbo.Conformities", "InputTeamName", c => c.String());
            this.DropForeignKey("dbo.TempResults", "TempSportEvent_Id", "dbo.TempSportEvents");
            this.DropForeignKey("dbo.TempSportEvents", "SportType_Id", "dbo.SportTypes");
            this.DropForeignKey("dbo.TempResults", "Team_Id", "dbo.Teams");
            this.DropForeignKey("dbo.Conformities", "TempResult_Id", "dbo.TempResults");
            this.DropIndex("dbo.TempSportEvents", new[] { "SportType_Id" });
            this.DropIndex("dbo.TempResults", new[] { "TempSportEvent_Id" });
            this.DropIndex("dbo.TempResults", new[] { "Team_Id" });
            this.DropIndex("dbo.Conformities", new[] { "TempResult_Id" });
            this.DropColumn("dbo.Conformities", "TempResult_Id");
            this.DropColumn("dbo.Conformities", "ExistedName");
            this.DropColumn("dbo.Conformities", "InputName");
            this.DropTable("dbo.TempSportEvents");
            this.DropTable("dbo.TempResults");
            this.CreateIndex("dbo.Conformities", "ExistedTeam_Id");
            this.AddForeignKey("dbo.Conformities", "ExistedTeam_Id", "dbo.Teams", "Id");
        }
    }
}
