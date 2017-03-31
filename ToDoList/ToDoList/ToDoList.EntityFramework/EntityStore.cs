using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ToDoList.EntityFramework
{
	internal sealed class EntityStore
	{
		private DbContext DbContext { get; }

		public EntityStore(DbContext dbContext)
		{
			if (dbContext == null)
			{
				throw new ArgumentNullException(nameof(dbContext));
			}
			DbContext = dbContext;
		}

		public IQueryable<TEntity> Query<TEntity>() where TEntity : class => DbContext.Set<TEntity>().AsNoTracking();

		public IQueryable<TEntity> QueryLocal<TEntity>() where TEntity : class => DbContext.Set<TEntity>();

		public TEntity Find<TEntity>(params object[] keyValues) where TEntity : class => DbContext.Set<TEntity>().Find(keyValues);

		public async Task<TEntity> FindAsync<TEntity>(params object[] keyValues) where TEntity : class => await DbContext.Set<TEntity>().FindAsync(keyValues);

		public TEntity Add<TEntity>(TEntity entity)
			where TEntity : class
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}
			return DbContext.Set<TEntity>().Add(entity);
		}

		public IEnumerable<TEntity> AddRange<TEntity>(IEnumerable<TEntity> entities)
			where TEntity : class
		{
			if (entities == null)
			{
				throw new ArgumentNullException(nameof(entities));
			}
			return DbContext.Set<TEntity>().AddRange(entities);
		}

		private void UpdateInternal<TEntity>(TEntity current, TEntity updated)
			where TEntity : class
		{
			var entry = DbContext.Entry(current);
			entry.State = EntityState.Modified;
			entry.CurrentValues.SetValues(updated);
			return;
		}

		public TEntity Update<TEntity>(TEntity current, TEntity updated)
			where TEntity : class
		{
			if (current == null)
			{
				throw new ArgumentNullException(nameof(current));
			}

			if (updated == null)
			{
				throw new ArgumentNullException(nameof(updated));
			}

			UpdateInternal(current, updated);

			return current;
		}

		public IList<TEntity> UpdateRange<TEntity>(IList<TEntity> currentEntities, IList<TEntity> updatedEntities)
			where TEntity : class
		{
			if (currentEntities == null)
			{
				throw new ArgumentNullException(nameof(currentEntities));
			}

			if (updatedEntities == null)
			{
				throw new ArgumentNullException(nameof(updatedEntities));
			}

			var count = currentEntities.Count;

			if (updatedEntities.Count != count)
			{
				throw new ArgumentException("the counts don't match.", nameof(updatedEntities));
			}

			for (var index = 0; index != count; ++index)
			{
				UpdateInternal(currentEntities[index], updatedEntities[index]);
			}

			return currentEntities;
		}

		private void UpdateInternal<TEntity>(TEntity current, TEntity updated, Func<TEntity, object> propertiesToUpdate)
			where TEntity : class
		{
			var entry = DbContext.Entry<TEntity>(current);
			entry.State = EntityState.Modified;
			entry.CurrentValues.SetValues(propertiesToUpdate(updated));
			return;
		}

		public TEntity Update<TEntity>(TEntity current, TEntity updated, Func<TEntity, object> propertiesToUpdate)
			where TEntity : class
		{
			if (current == null)
			{
				throw new ArgumentNullException(nameof(current));
			}

			if (updated == null)
			{
				throw new ArgumentNullException(nameof(updated));
			}

			UpdateInternal(current, updated, propertiesToUpdate);

			return current;
		}

		public IList<TEntity> UpdateRange<TEntity>(IList<TEntity> currentEntities, IList<TEntity> updatedEntities, Func<TEntity, object> propertiesToUpdate)
			where TEntity : class
		{
			if (currentEntities == null)
			{
				throw new ArgumentNullException(nameof(currentEntities));
			}

			if (updatedEntities == null)
			{
				throw new ArgumentNullException(nameof(updatedEntities));
			}

			var count = currentEntities.Count;

			if (updatedEntities.Count != count)
			{
				throw new ArgumentException("the counts don't match.", nameof(updatedEntities));
			}

			for (var index = 0; index != count; ++index)
			{
				UpdateInternal(currentEntities[index], updatedEntities[index], propertiesToUpdate);
			}

			return currentEntities;
		}

		private static IEnumerable<PropertyInfo> EnumerateNavigationPropertyInfos(IEnumerable<PropertyInfo> propertyInfos)
		{
			var query =
				from propertyInfo in propertyInfos
				let typeInfo = propertyInfo.PropertyType.GetTypeInfo()
				where typeInfo.IsAbstract && typeInfo.IsGenericType(typeof(ICollection<>))
				select propertyInfo;
			return query;
		}

		private static IEnumerable<PropertyInfo> EnumerateNavigationPropertyInfos<TEntity>()
		{
			var propertyInfos = ReflectionExtensions.GetPropertyInfos<TEntity>();
			return EnumerateNavigationPropertyInfos(propertyInfos);
		}

		private static IEnumerable<PropertyInfo> EnumerateNavigationPropertyInfos(Type entityType)
		{
			var propertyInfos = ReflectionExtensions.GetPropertyInfos(entityType);
			return EnumerateNavigationPropertyInfos(propertyInfos);
		}

		private static void RemoveRangeCascade(DbContext dbContext, IList entities)
		{
			var entityType = entities[0].GetType();
			var navigationPropertyInfos = EnumerateNavigationPropertyInfos(entityType).ToList();

			foreach (var entity in entities)
			{
				var enumerablesQuery =
					from navigationPropertyInfo in navigationPropertyInfos
					let enumerable = navigationPropertyInfo.GetValue(entity) as IEnumerable
					where enumerable != null
					select enumerable;

				foreach (var enumerable in enumerablesQuery)
				{
					var entities0 = enumerable.Cast<object>().ToList();

					if (entities0.Count != 0)
					{
						RemoveRangeCascade(dbContext, entities0);
					}
				}

				dbContext.Entry(entity).State = EntityState.Deleted;
			}

			dbContext.Set(entityType).RemoveRange(entities);

			return;
		}

		private static void Remove<TEntity>(DbContext dbContext, IList<PropertyInfo> navigationPropertyInfos, TEntity entity, bool cascade)
			where TEntity : class
		{
			if (cascade)
			{
				var enumerablesQuery =
					from navigationPropertyInfo in navigationPropertyInfos
					let enumerable = navigationPropertyInfo.GetValue(entity) as IEnumerable
					where enumerable != null
					select enumerable;

				foreach (var enumerable in enumerablesQuery)
				{
					var entities = enumerable.Cast<object>().ToList();

					if (entities.Count != 0)
					{
						RemoveRangeCascade(dbContext, entities);
					}
				}
			}

			dbContext.Entry(entity).State = EntityState.Deleted;

			return;
		}

		public TEntity Remove<TEntity>(TEntity entity, bool cascade)
			where TEntity : class
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			var dbContext = DbContext;
			var navigationPropertyInfos = EnumerateNavigationPropertyInfos<TEntity>().ToList();
			Remove(dbContext, navigationPropertyInfos, entity, cascade);

			return dbContext.Set<TEntity>().Remove(entity);
		}

		public IEnumerable<TEntity> RemoveRange<TEntity>(IEnumerable<TEntity> entities, bool cascade)
			where TEntity : class
		{
			if (entities == null)
			{
				throw new ArgumentNullException(nameof(entities));
			}

			var dbContext = DbContext;
			var navigationPropertyInfos = EnumerateNavigationPropertyInfos<TEntity>().ToList();

			foreach (var entity in entities)
			{
				Remove(dbContext, navigationPropertyInfos, entity, cascade);
			}

			return dbContext.Set<TEntity>().RemoveRange(entities);
		}

		public int SaveChanges() => DbContext.SaveChanges();

		public async Task<int> SaveChangesAsync() => await DbContext.SaveChangesAsync();

		public int UndoChanges()
		{
			var changeTracker = DbContext.ChangeTracker;

			if (!changeTracker.HasChanges())
			{
				return 0;
			}

			var undoCount = 0;

			foreach (var entry in changeTracker.Entries())
			{
				switch (entry.State)
				{
					case EntityState.Modified:
						entry.CurrentValues.SetValues(entry.OriginalValues);
						entry.State = EntityState.Unchanged;
						++undoCount;
						break;
					case EntityState.Deleted:
						entry.State = EntityState.Unchanged;
						++undoCount;
						break;
					case EntityState.Added:
						entry.State = EntityState.Detached;
						++undoCount;
						break;
				}
			}

			return undoCount;
		}
	}
}