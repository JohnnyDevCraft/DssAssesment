using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.EntityFramework
{
	public class Task : ITask
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[StringLength(30)]
		public string Name { get; set; }

		[StringLength(250)]
		public string Description { get; set; }

		public int Priority { get; set; }

		[Column(TypeName = "date")]
		public DateTime DueDate { get; set; }

		public bool IsCompleted { get; set; }

        [Column(TypeName ="datetime2")]
        public DateTime? CompletedAt { get; set; }
    }
}