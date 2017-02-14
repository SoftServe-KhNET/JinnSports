namespace JinnSports.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClaimsDeleted : DbMigration
    {
        public override void Up()
        {
            this.DropForeignKey("dbo.Claim", "UserId", "dbo.User");
            this.DropIndex("dbo.Claim", new[] { "UserId" });
            this.DropTable("dbo.Claim");
        }
        
        public override void Down()
        {
            this.CreateTable(
                "dbo.Claim",
                c => new
                    {
                        ClaimId = c.Int(nullable: false, identity: true),
                        UserId = c.Guid(nullable: false),
                        ClaimType = c.String(maxLength: 4000),
                        ClaimValue = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.ClaimId);

            this.CreateIndex("dbo.Claim", "UserId");
            this.AddForeignKey("dbo.Claim", "UserId", "dbo.User", "UserId", cascadeDelete: true);
        }
    }
}
