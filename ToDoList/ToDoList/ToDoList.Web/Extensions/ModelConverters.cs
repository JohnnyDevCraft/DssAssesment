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
    }
}