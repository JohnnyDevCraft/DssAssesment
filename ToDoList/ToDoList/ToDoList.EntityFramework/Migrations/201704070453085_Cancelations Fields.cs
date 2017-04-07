namespace ToDoList.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CancelationsFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "Canceled", c=>c.Boolean(nullable:false, defaultValue:false));
            AddColumn("dbo.Tasks", "CancelReason", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "CancelReason");
            DropColumn("dbo.Tasks", "Canceled");
        }
    }
}
