namespace JinnSports.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            this.CreateTable(
                "dbo.CompetitionEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            this.CreateTable(
                "dbo.Results",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Score = c.String(),
                        CompetitionEvent_Id = c.Int(),
                        Team_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CompetitionEvents", t => t.CompetitionEvent_Id)
                .ForeignKey("dbo.Teams", t => t.Team_Id)
                .Index(t => t.CompetitionEvent_Id)
                .Index(t => t.Team_Id);
            
            this.CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SportType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SportTypes", t => t.SportType_Id)
                .Index(t => t.SportType_Id);
            
            this.CreateTable(
                "dbo.SportTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            this.DropForeignKey("dbo.Teams", "SportType_Id", "dbo.SportTypes");
            this.DropForeignKey("dbo.Results", "Team_Id", "dbo.Teams");
            this.DropForeignKey("dbo.Results", "CompetitionEvent_Id", "dbo.CompetitionEvents");
            this.DropIndex("dbo.Teams", new[] { "SportType_Id" });
            this.DropIndex("dbo.Results", new[] { "Team_Id" });
            this.DropIndex("dbo.Results", new[] { "CompetitionEvent_Id" });
            this.DropTable("dbo.SportTypes");
            this.DropTable("dbo.Teams");
            this.DropTable("dbo.Results");
            this.DropTable("dbo.CompetitionEvents");
        }
    }
}
