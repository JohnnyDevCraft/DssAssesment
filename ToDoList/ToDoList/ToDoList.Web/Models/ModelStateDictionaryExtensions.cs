using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace ToDoList.Web
{
	public static class ModelStateDictionaryExtensions
	{
		public static void AddRange(this ModelStateDictionary modelStateDictionary, IEnumerable<ValidationResult> validationResults)
		{
			if (modelStateDictionary == null)
			{
				throw new ArgumentNullException(nameof(modelStateDictionary));
			}

			if (validationResults == null)
			{
				throw new ArgumentNullException(nameof(validationResults));
			}

			foreach (var validationResult in validationResults)
			{
				var errorMessage = validationResult.ErrorMessage;
				var memberNames = validationResult.MemberNames;

				if (memberNames.Any())
				{
					foreach (var memberName in memberNames)
					{
						modelStateDictionary.AddModelError(memberName, errorMessage);
					}
				}
				else
				{
					modelStateDictionary.AddModelError(string.Empty, errorMessage);
				}
			}

			return;
		}
	}
}