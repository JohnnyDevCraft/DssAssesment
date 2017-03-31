using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList
{
	public interface IAsyncService
	{
		Task<bool> AnyAsync<TItem>(IQueryable<TItem> items);

		Task<TItem> FirstOrDefaultAsync<TItem>(IQueryable<TItem> items);

		Task<List<TItem>> ToListAsync<TItem>(IQueryable<TItem> items);
	}
}