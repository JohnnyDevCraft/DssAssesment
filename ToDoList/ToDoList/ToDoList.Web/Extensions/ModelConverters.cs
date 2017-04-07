using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToDoList.EntityFramework;
using ToDoList.Web.Models;

namespace ToDoList.Web.Extensions
{
    public static class ModelConverters
    {
        public static CategoryViewModel ToViewModel(this Category c)
        {
            return new CategoryViewModel()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            };
        }

        public static Category ToDataModel(this CategoryViewModel vm)
        {
            return new Category()
            {
                Id = vm.Id,
                Name = vm.Name,
                Description = vm.Description
            };
        }

        public static Task ToDataModel(this TaskJsonViewModel vm)
        {
            var t = new Task()
            {
                Id = vm.Id,
                Name = vm.Name,
                Description = vm.Description,
                CategoryId = int.Parse(vm.CategoryId),
                Priority = vm.Priority,
                IsCompleted = vm.Completed
            };

            if (vm.CompletedAt != null) t.CompletedAt = DateTime.Parse(vm.CompletedAt);
            if (vm.DueDate != null) t.DueDate = DateTime.Parse(vm.DueDate);

            return t;

        }

        public static TaskJsonViewModel ToViewModel(this Task task)
        {
            return new TaskJsonViewModel()
            {
                CategoryId = task.CategoryId.ToString(),
                CompletedAt = task.CompletedAt?.ToString("G") ?? "",
                Completed = task.IsCompleted,
                Description = task.Description,
                DueDate = task.DueDate.ToString("G"),
                Id = task.Id,
                Name = task.Name,
                Priority = task.Priority,
                Category = task.Category.Name
            };
        }
    }
}