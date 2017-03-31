using System;
using System.Collections.Generic;
using System.Linq;
using Tasks = System.Threading.Tasks;

namespace ToDoList.EntityFramework
{
	public class TaskStore<TTask> : ITaskStore<TTask>
		where TTask : class, ITask
	{
		private EntityStore Store { get; }

		public TaskStore(ToDoListDbContext dbContext)
		{
			Store = new EntityStore(dbContext);
		}

		public IQueryable<TTask> Query() => Store.Query<TTask>();

		public IQueryable<TTask> QueryLocal() => Store.QueryLocal<TTask>();

		public TTask Find(params object[] keyValues) => Store.Find<TTask>(keyValues);

		public async Tasks.Task<TTask> FindAsync(params object[] keyValues) => await Store.FindAsync<TTask>(keyValues);

		public TTask Add(TTask task) => Store.Add(task);

		public IEnumerable<TTask> AddRange(IEnumerable<TTask> tasks) => Store.AddRange(tasks);

		public TTask Update(TTask current, TTask updated) => Store.Update(current, updated);

		public IList<TTask> UpdateRange(IList<TTask> currentTasks, IList<TTask> updatedTasks) => Store.UpdateRange(currentTasks, updatedTasks);

		public TTask Update(TTask current, TTask updated, Func<TTask, object> propertiesToUpdate) => Store.Update(current, updated, propertiesToUpdate);

		public IList<TTask> UpdateRange(IList<TTask> currentTasks, IList<TTask> updatedTasks, Func<TTask, object> propertiesToUpdate)
			=> Store.UpdateRange(currentTasks, updatedTasks, propertiesToUpdate);

		public TTask Remove(TTask task, bool cascade) => Store.Remove(task, cascade);

		public IEnumerable<TTask> RemoveRange(IEnumerable<TTask> tasks, bool cascade) => Store.RemoveRange(tasks, cascade);

		public int SaveChanges() => Store.SaveChanges();

		public async Tasks.Task<int> SaveChangesAsync() => await Store.SaveChangesAsync();

		public int UndoChanges() => Store.UndoChanges();
	}
}