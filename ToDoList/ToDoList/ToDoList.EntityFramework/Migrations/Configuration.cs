namespace ToDoList.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ToDoList.EntityFramework.ToDoListDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "ToDoList.EntityFramework.ToDoListDbContext";
        }

        protected override void Seed(ToDoList.EntityFramework.ToDoListDbContext context)
        {
            context.Categories.Add(new Category() {Name = "Administration", Description = "Administration Task"});
        }
    }
}
