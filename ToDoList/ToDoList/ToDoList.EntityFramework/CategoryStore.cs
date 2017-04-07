using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.EntityFramework
{
    public class CategoryStore<TCategory> : ICategoryStore<TCategory>
        where TCategory : class, ICategory
    {

        private EntityStore _store { get; }

        public CategoryStore(ToDoListDbContext context)
        {
            _store = new EntityStore(context);
        }

        public TCategory Add(TCategory category) => _store.Add(category);

        public TCategory Remove(TCategory category, bool cascade) => _store.Remove(category, cascade);

        public TCategory Find(params object[] keyValues) => _store.Find<TCategory>(keyValues);

        public Task<TCategory> FindAsync(params object[] keyValues) => _store.FindAsync<TCategory>(keyValues);

        public IQueryable<TCategory> Query() => _store.Query<TCategory>();

        public IQueryable<TCategory> QueryLocal() => _store.QueryLocal<TCategory>();

        public int SaveChanges() => _store.SaveChanges();

        public Task<int> SaveChangesAsync() => _store.SaveChangesAsync();

        public int UndoChanges() => _store.UndoChanges();

        public TCategory Update(TCategory current, TCategory updated) => _store.Update(current, updated);

        public IEnumerable<TCategory> RemoveRange(IEnumerable<TCategory> categories, bool cascade) => _store.RemoveRange(categories, cascade);
    }
}
