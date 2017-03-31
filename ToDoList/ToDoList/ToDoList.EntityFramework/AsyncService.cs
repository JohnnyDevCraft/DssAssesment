using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.EntityFramework
{
	public sealed class AsyncService : IAsyncService
	{
		public static AsyncService Default { get; } = new AsyncService();

		public Task<bool> AnyAsync<TItem>(IQueryable<TItem> items) => items.AnyAsync();

		public Task<TItem> FirstOrDefaultAsync<TItem>(IQueryable<TItem> items) => items.FirstOrDefaultAsync();

		public Task<List<TItem>> ToListAsync<TItem>(IQueryable<TItem> items) => items.ToListAsync();
	}
}
