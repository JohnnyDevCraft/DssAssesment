using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Web.Models
{
	public sealed class CreateTaskViewModel
	{
		[Required]
		public string Name { get; set; }

		public string Description { get; set; }

		[Required]
		[Range(1, int.MaxValue)]
		public int? Priority { get; set; }

		[Required]
		[Display(Name = "Due Date")]
		public DateTime? DueDate { get; set; }
	}
}