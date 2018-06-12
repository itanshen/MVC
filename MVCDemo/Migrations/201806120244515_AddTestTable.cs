namespace MVCDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTestTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SysUserRole", "SysRoleID", "dbo.SysRole");
            DropForeignKey("dbo.SysUserRole", "SysUserID", "dbo.SysUser");
            CreateTable(
                "dbo.Test",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TestName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddForeignKey("dbo.SysUserRole", "SysRoleID", "dbo.SysRole", "ID", cascadeDelete: true);
            AddForeignKey("dbo.SysUserRole", "SysUserID", "dbo.SysUser", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SysUserRole", "SysUserID", "dbo.SysUser");
            DropForeignKey("dbo.SysUserRole", "SysRoleID", "dbo.SysRole");
            DropTable("dbo.Test");
            AddForeignKey("dbo.SysUserRole", "SysUserID", "dbo.SysUser", "ID");
            AddForeignKey("dbo.SysUserRole", "SysRoleID", "dbo.SysRole", "ID");
        }
    }
}
