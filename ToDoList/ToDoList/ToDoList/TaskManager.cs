using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList
{
	public sealed class TaskManager<TTask>
		where TTask : class, ITask
	{
		private ITaskStore<TTask> Store { get; }

		public TaskManager(ITaskStore<TTask> store)
		{
			Store = store;
		}

        public async Task<AddResult> AddAsync(TTask task)
        {
            var validationResults = Validate(task);

            if (validationResults.HasErrors())
            {
                return new AddResult { ValidationResults = validationResults };
            }

            Store.Add(task);
            await Store.SaveChangesAsync();

            return new AddResult { Id = task.Id };
        }

        public async Task<bool> Delete(TTask task)
        {
            var success = Store.Remove(task, true);
            await Store.SaveChangesAsync();

            return true;
        }

        public async Task<IList<TTask>> GetAllAsync() => await Services.Async.ToListAsync(Store.Query());

        public async Task<TTask> GetTaskById(int id) => await Store.FindAsync(id);

        public async Task<IList<ValidationResult>> Update(TTask task)
        {
            var validationResults = Validate(task);
            if (validationResults.HasErrors())
            {
                return validationResults;
            }

            var currentTask = await Store.FindAsync(task.Id);
            if (currentTask == null)
            {
                throw new InvalidOperationException($"Task {task.Id} does not exist.");
            }

            Store.Update(currentTask, task);
            Store.SaveChanges();

            return null;
        }

		private static IEnumerable<ValidationResult> ValidationInternal(ITask task)
		{
			if (string.IsNullOrWhiteSpace(task.Name))
			{
				yield return new ValidationResult("Name is required.", new[] { nameof(task.Name) });
			}
            if (task.Name?.Length > 30)
            {
                yield return new ValidationResult("Name cannot be longer than 30 characters", new[] {nameof(task.Name)});
            }
            if (task.Description?.Length > 250)
            {
                yield return new ValidationResult("Description cannot be longer than 250 characters", new[] { nameof(task.Description) });
            }
			if (task.Priority < 1)
			{
				yield return new ValidationResult("Priority can't be less than 1.", new[] { nameof(task.Priority) });
			}
			if (task.DueDate.Date < DateTime.Today)
			{
				yield return new ValidationResult("Due Date can't be a past date.", new[] { nameof(task.DueDate) });
			}
            if (task.CategoryId == 0)
            {
                yield return new ValidationResult("You must have a valid category selected.", new[] { nameof(task.CategoryId) });
            }
			yield break;
		}

        public static IList<ValidationResult> Validate(TTask task) => ValidationInternal(task).ToList();
	}
}