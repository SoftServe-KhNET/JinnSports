namespace JinnSports.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeletedNullableScores : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TempResults", "Score", c => c.Int(nullable: false));
            AlterColumn("dbo.Results", "Score", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Results", "Score", c => c.Int());
            AlterColumn("dbo.TempResults", "Score", c => c.Int());
        }
    }
}
