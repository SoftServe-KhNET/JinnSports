namespace JinnSports.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConformConfirmationDelete : DbMigration
    {
        public override void Up()
        {
            this.DropColumn("dbo.Conformities", "IsConfirmed");
        }
        
        public override void Down()
        {
            this.AddColumn("dbo.Conformities", "IsConfirmed", c => c.Boolean(nullable: false));
        }
    }
}
