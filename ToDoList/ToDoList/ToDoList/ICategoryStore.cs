using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList
{
    public interface ICategoryStore<TCategory>
        where TCategory : class, ICategory
    {
        IQueryable<TCategory> Query();
        IQueryable<TCategory> QueryLocal();

        TCategory Find(params object[] keyValues);
        Task<TCategory> FindAsync(params object[] keyValues);
        TCategory Add(TCategory category);
        TCategory Update(TCategory current, TCategory updated);
        TCategory Remove(TCategory category, bool cascade);
        IEnumerable<TCategory> RemoveRange(IEnumerable<TCategory> categories, bool cascade);

        int SaveChanges();
        Task<int> SaveChangesAsync();
        int UndoChanges();

    }
}
