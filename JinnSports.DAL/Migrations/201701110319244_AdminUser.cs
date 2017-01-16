namespace JinnSports.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AdminUser : DbMigration
    {
        public override void Up()
        {
            this.Sql(@"INSERT INTO [dbo].[User] ([USERID], [USERNAME], [PASSWORDHASH], [SECURITYSTAMP]) VALUES('D91CB2FE-70D3-42AD-AF5B-1E3C4A0D50BD', 'admin', 'ALpR2wYzw+gSsXMqF27HISgtlMGPEmsUukD72aHLSnmOU7sc1fK6PxE99nJEUkWrpw==', '9a0abb69-be43-4b82-b70a-830649c71fba')
                  INSERT INTO [dbo].[Role] ([ROLEID], [NAME]) VALUES('239A8158-FB2D-4F4F-8F6D-BE7345738F61', 'admin')
                  INSERT INTO [dbo].[UserRole] ([ROLEID], [USERID]) VALUES('239A8158-FB2D-4F4F-8F6D-BE7345738F61', 'D91CB2FE-70D3-42AD-AF5B-1E3C4A0D50BD')");
        }
        
        public override void Down()
        {
        }
    }
}
