namespace JinnSports.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Predictor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventPredictions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HomeWinProbability = c.Double(nullable: false),
                        DrawProbability = c.Double(nullable: false),
                        AwayWinProbability = c.Double(nullable: false),
                        AwayTeam_Id = c.Int(),
                        HomeTeam_Id = c.Int(),
                        SportEvent_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.AwayTeam_Id)
                .ForeignKey("dbo.Teams", t => t.HomeTeam_Id)
                .ForeignKey("dbo.SportEvents", t => t.SportEvent_Id)
                .Index(t => t.AwayTeam_Id)
                .Index(t => t.HomeTeam_Id)
                .Index(t => t.SportEvent_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventPredictions", "SportEvent_Id", "dbo.SportEvents");
            DropForeignKey("dbo.EventPredictions", "HomeTeam_Id", "dbo.Teams");
            DropForeignKey("dbo.EventPredictions", "AwayTeam_Id", "dbo.Teams");
            DropIndex("dbo.EventPredictions", new[] { "SportEvent_Id" });
            DropIndex("dbo.EventPredictions", new[] { "HomeTeam_Id" });
            DropIndex("dbo.EventPredictions", new[] { "AwayTeam_Id" });
            DropTable("dbo.EventPredictions");
        }
    }
}
