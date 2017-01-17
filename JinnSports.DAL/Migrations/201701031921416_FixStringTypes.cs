namespace JinnSports.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixStringTypes : DbMigration
    {
        private const int STRINGSIZE = 255;

        public override void Up()
        {
            this.AlterColumn("dbo.SportTypes", "Name", c => c.String(maxLength: STRINGSIZE, nullable: false));
            this.AlterColumn("dbo.Teams", "Name", c => c.String(maxLength: STRINGSIZE, nullable: false));
            this.AlterColumn("dbo.TeamNames", "Name", c => c.String(maxLength: STRINGSIZE, nullable: false));
            this.AlterColumn("dbo.Conformities", "InputName", c => c.String(maxLength: STRINGSIZE, nullable: false));
            this.AlterColumn("dbo.Conformities", "ExistedName", c => c.String(maxLength: STRINGSIZE, nullable: false));
        }
        
        public override void Down()
        {
        }
    }
}
