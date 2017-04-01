using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ToDoList.Web.Models
{
	public sealed class TaskViewModel
	{
        public int Id { get; internal set; }

        public string Name { get; internal set; }

		public string Description { get; internal set; }
               
		public string Priority { get; internal set; }
        
		public string DueDate { get; internal set; }
        
		public string Completed { get; internal set; }

        [DataType(DataType.DateTime)]
        public string CompletedAt { get; internal set; }

        public TaskViewModel(ITask task)
        {
            Id = task.Id;
            Name = task.Name;
            Description = task.Description;
            Priority = task.Priority.ToString("N0", NumberFormatInfo.CurrentInfo);
            DueDate = task.DueDate.ToLongDateString();
            Completed = task.IsCompleted ? "Complete" : null;
            CompletedAt = task.CompletedAt.HasValue ? task.CompletedAt.Value.ToString("G") : "";
        }
	}
}