namespace JinnSports.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NameMatching : DbMigration
    {
        public override void Up()
        {
            this.DropIndex("dbo.SportEvents", new[] { "SportType_Id" });
            this.DropIndex("dbo.Results", new[] { "SportEvent_Id" });
            this.DropIndex("dbo.Results", new[] { "Team_Id" });
            this.DropIndex("dbo.Teams", new[] { "SportType_Id" });
            CreateTable(
                "dbo.Conformities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsConfirmed = c.Boolean(nullable: false),
                        ExistedTeam_Id = c.Int(),
                        InputTeam_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.ExistedTeam_Id)
                .ForeignKey("dbo.Teams", t => t.InputTeam_Id)
                .Index(t => t.ExistedTeam_Id)
                .Index(t => t.InputTeam_Id);
            
            CreateTable(
                "dbo.TeamNames",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Team_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.Team_Id)
                .Index(t => t.Team_Id);

            this.AddColumn("dbo.Results", "IsHome", c => c.Boolean(nullable: false));
            this.AlterColumn("dbo.SportEvents", "SportType_Id", c => c.Int(nullable: false));
            this.AlterColumn("dbo.Results", "Score", c => c.Int());
            this.AlterColumn("dbo.Results", "SportEvent_Id", c => c.Int(nullable: false));
            this.AlterColumn("dbo.Results", "Team_Id", c => c.Int(nullable: false));
            this.AlterColumn("dbo.Teams", "Name", c => c.String(nullable: false, maxLength: 30));
            this.AlterColumn("dbo.Teams", "SportType_Id", c => c.Int(nullable: false));
            this.AlterColumn("dbo.SportTypes", "Name", c => c.String(nullable: false));
            this.CreateIndex("dbo.Teams", "SportType_Id");
            this.CreateIndex("dbo.Results", "SportEvent_Id");
            this.CreateIndex("dbo.Results", "Team_Id");
            this.CreateIndex("dbo.SportEvents", "SportType_Id");
        }
        
        public override void Down()
        {
            this.DropForeignKey("dbo.Conformities", "InputTeam_Id", "dbo.Teams");
            this.DropForeignKey("dbo.Conformities", "ExistedTeam_Id", "dbo.Teams");
            this.DropForeignKey("dbo.TeamNames", "Team_Id", "dbo.Teams");
            this.DropIndex("dbo.SportEvents", new[] { "SportType_Id" });
            this.DropIndex("dbo.Results", new[] { "Team_Id" });
            this.DropIndex("dbo.Results", new[] { "SportEvent_Id" });
            this.DropIndex("dbo.TeamNames", new[] { "Team_Id" });
            this.DropIndex("dbo.Teams", new[] { "SportType_Id" });
            this.DropIndex("dbo.Conformities", new[] { "InputTeam_Id" });
            this.DropIndex("dbo.Conformities", new[] { "ExistedTeam_Id" });
            this.AlterColumn("dbo.SportTypes", "Name", c => c.String());
            this.AlterColumn("dbo.Teams", "SportType_Id", c => c.Int());
            this.AlterColumn("dbo.Teams", "Name", c => c.String());
            this.AlterColumn("dbo.Results", "Team_Id", c => c.Int());
            this.AlterColumn("dbo.Results", "SportEvent_Id", c => c.Int());
            this.AlterColumn("dbo.Results", "Score", c => c.Int(nullable: false));
            this.AlterColumn("dbo.SportEvents", "SportType_Id", c => c.Int());
            this.DropColumn("dbo.Results", "IsHome");
            this.DropTable("dbo.TeamNames");
            this.DropTable("dbo.Conformities");
            this.CreateIndex("dbo.Teams", "SportType_Id");
            this.CreateIndex("dbo.Results", "Team_Id");
            this.CreateIndex("dbo.Results", "SportEvent_Id");
            this.CreateIndex("dbo.SportEvents", "SportType_Id");
        }
    }
}
