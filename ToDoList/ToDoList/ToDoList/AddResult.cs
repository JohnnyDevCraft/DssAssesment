using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ToDoList
{
	public sealed class AddResult
	{
		public int? Id { get; set; }

		public IList<ValidationResult> ValidationResults { get; set; }
	}
}