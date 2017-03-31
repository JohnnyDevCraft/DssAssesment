using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ToDoList
{
	public static class ValidationResultExtensions
	{
		public static bool HasErrors(this ICollection<ValidationResult> items)
		{
			if (items == null)
			{
				return false;
			}
			return items.Count != 0;
		}
	}
}