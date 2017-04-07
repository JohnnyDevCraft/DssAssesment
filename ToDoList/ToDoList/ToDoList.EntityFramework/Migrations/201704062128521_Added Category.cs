namespace ToDoList.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "CategoryId", c => c.Int(nullable: true));
            Sql("SET IDENTITY_INSERT dbo.Categories ON; " +
                "Delete From dbo.Categories Where Id = 1;" +
                "INSERT INTO dbo.Categories " +
                "(Id, Name, Description)" +
                "Values(1, 'Default', 'Default Cat'); " +
                "UPDATE dbo.Tasks SET CategoryId = 1;" +
                "SET IDENTITY_INSERT dbo.Categories OFF;");
            AlterColumn("dbo.Tasks","CategoryId", c=>c.Int(nullable: false) );
            CreateIndex("dbo.Tasks", "CategoryId");
            AddForeignKey("dbo.Tasks", "CategoryId", "dbo.Categories", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Tasks", new[] { "CategoryId" });
            DropColumn("dbo.Tasks", "CategoryId");
        }
    }
}
