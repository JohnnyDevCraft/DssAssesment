using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList
{
    public class CategoryManager<TCategory>
        where TCategory : class, ICategory
    {
        private ICategoryStore<TCategory> _store { get; }

        public CategoryManager(ICategoryStore<TCategory> store)
        {
            _store = store;
        }

        public async Task<AddResult> AddAsync(TCategory category)
        {
            var validationResults = Validate(category);

            if (validationResults.HasErrors())
            {
                return new AddResult { ValidationResults = validationResults };
            }

            _store.Add(category);

            await _store.SaveChangesAsync();

            return new AddResult { Id = category.Id };
        }

        public async Task<bool> Delete(TCategory category)
        {
            var success = _store.Remove(category, false);
            await _store.SaveChangesAsync();

            return true;
        }

        public async Task<IList<TCategory>> GetAllAsync() => await Services.Async.ToListAsync(_store.Query());

        public async Task<TCategory> GetCategoryByIdAsync(int id) => await _store.FindAsync(id);

        public async Task<IList<ValidationResult>> Update(TCategory category)
        {
            var validationResults = Validate(category);
            if (validationResults.HasErrors())
            {
                return validationResults;
            }

            var currentCategory = await _store.FindAsync(category.Id);

            if (currentCategory == null)
            {
                throw new InvalidOperationException($"Category {category.Id} does not exist.");
            }

            _store.Update(currentCategory, category);
            await _store.SaveChangesAsync();

            return null;
        }



        private static IEnumerable<ValidationResult> ValidationInternal(TCategory category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                yield return new ValidationResult("Name is required", new[] { nameof(category.Name) });
            }
            if (category.Name?.Length > 30)
            {
                yield return new ValidationResult("Name cannot be longer than 30 characters", new[] { nameof(category.Name) });
            }
            if (category.Description?.Length < 250)
            {
                yield return new ValidationResult("Description cannot be longer than 250 characters.", new[] { nameof(category.Description) });
            }
        }

        public static IList<ValidationResult> Validate(TCategory category) => ValidationInternal(category).ToList();
    }
}
