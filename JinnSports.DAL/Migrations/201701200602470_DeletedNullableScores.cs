namespace JinnSports.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeletedNullableScores : DbMigration
    {
        public override void Up()
        {
            this.AlterColumn("dbo.TempResults", "Score", c => c.Int(nullable: false));
            this.AlterColumn("dbo.Results", "Score", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            this.AlterColumn("dbo.Results", "Score", c => c.Int());
            this.AlterColumn("dbo.TempResults", "Score", c => c.Int());
        }
    }
}
