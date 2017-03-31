namespace ToDoList.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newfieldCompletedAtaddedtoTasktable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "CompletedAt", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "CompletedAt");
        }
    }
}
