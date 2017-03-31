using System.Data.Entity;

namespace ToDoList.EntityFramework
{
	public class ToDoListDbContext : DbContext
	{
		public DbSet<Task> Tasks { get; set; }

		public ToDoListDbContext() :
			base("name=ToDoList")
		{
		}
	}
}