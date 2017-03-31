using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList
{
	public interface ITaskStore<TTask>
		where TTask : class, ITask
	{
		IQueryable<TTask> Query();

		IQueryable<TTask> QueryLocal();

		TTask Find(params object[] keyValues);

		Task<TTask> FindAsync(params object[] keyValues);

		TTask Add(TTask task);

		IEnumerable<TTask> AddRange(IEnumerable<TTask> tasks);

		TTask Update(TTask current, TTask updated);

		IList<TTask> UpdateRange(IList<TTask> currentTasks, IList<TTask> updatedTasks);

		TTask Update(TTask current, TTask updated, Func<TTask, object> propertiesToUpdate);

		IList<TTask> UpdateRange(IList<TTask> currentTasks, IList<TTask> updatedTasks, Func<TTask, object> propertiesToUpdate);

		TTask Remove(TTask task, bool cascade);

		IEnumerable<TTask> RemoveRange(IEnumerable<TTask> tasks, bool cascade);

		int SaveChanges();

		Task<int> SaveChangesAsync();

		int UndoChanges();
	}
}