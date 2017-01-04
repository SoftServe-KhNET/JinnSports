namespace JinnSports.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntputTeamNameStringConformity : DbMigration
    {
        public override void Up()
        {
            this.DropForeignKey("dbo.Conformities", "InputTeam_Id", "dbo.Teams");
            this.DropIndex("dbo.Conformities", new[] { "InputTeam_Id" });
            this.AddColumn("dbo.Conformities", "InputTeamName", c => c.String());
            this.DropColumn("dbo.Conformities", "InputTeam_Id");
        }
        
        public override void Down()
        {
            this.AddColumn("dbo.Conformities", "InputTeam_Id", c => c.Int());
            this.DropColumn("dbo.Conformities", "InputTeamName");
            this.CreateIndex("dbo.Conformities", "InputTeam_Id");
            this.AddForeignKey("dbo.Conformities", "InputTeam_Id", "dbo.Teams", "Id");
        }
    }
}
