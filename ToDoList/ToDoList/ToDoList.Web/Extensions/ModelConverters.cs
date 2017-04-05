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
            return new Task()
            {
                Id = vm.Id,
                Name = vm.Name,
                Description = vm.Description,
                CategoryId = vm.CategoryId,
                CompletedAt = vm.CompletedAt,
                DueDate = vm.DueDate,
                IsCompleted = vm.Completed,
                Priority = vm.Priority
            };
        }

        public static TaskJsonViewModel ToViewModel(this Task task)
        {
            return new TaskJsonViewModel()
            {
                CategoryId = task.CategoryId,
                CompletedAt = task.CompletedAt,
                Completed = task.IsCompleted,
                Description = task.Description,
                DueDate = task.DueDate,
                Id = task.Id,
                Name = task.Name,
                Priority = task.Priority
            };
        }
    }
}