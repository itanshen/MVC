namespace MVCDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MaxLengthOnNames1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SysUser", "UserName", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.SysUser", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.SysUser", "Password", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SysUser", "Password", c => c.String());
            AlterColumn("dbo.SysUser", "Email", c => c.String());
            AlterColumn("dbo.SysUser", "UserName", c => c.String(maxLength: 10));
        }
    }
}
