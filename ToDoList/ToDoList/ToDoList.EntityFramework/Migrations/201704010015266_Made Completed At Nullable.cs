namespace ToDoList.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MadeCompletedAtNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tasks", "CompletedAt", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tasks", "CompletedAt", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
    }
}
