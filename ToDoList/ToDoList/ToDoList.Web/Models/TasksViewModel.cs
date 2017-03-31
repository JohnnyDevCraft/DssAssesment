using System.Collections.Generic;

namespace ToDoList.Web.Models
{
	public sealed class TasksViewModel
	{
		public IList<TaskViewModel> Tasks { get; internal set; }
	}
}