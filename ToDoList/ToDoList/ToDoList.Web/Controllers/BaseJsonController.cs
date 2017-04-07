using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToDoList.Web.Models;

namespace ToDoList.Web.Controllers
{
    public abstract class BaseJsonController<T> : Controller
    {
        protected JsonNoticeViewModel<List<T>> GetModels(List<T> objs)
        {
            return new JsonNoticeViewModel<List<T>>()
            {
                Result = "Success",
                Value = objs
            };
        }

        protected JsonNoticeViewModel<T> GetModel(T obj)
        {
            return new JsonNoticeViewModel<T>()
            {
                Result = "Success",
                Value = obj
            };
        }

        protected JsonNoticeViewModel<T> GetModel(Exception ex)
        {
            var result = new JsonNoticeViewModel<T>()
            {
                Result = "Failure",
                ValidationResults = new List<string>(new [] {ex.Message})
            };

            GetInnerExceptions("", ex, result.ValidationResults);

            return result;
        }

        private void GetInnerExceptions(string start, Exception e, IList<string> errorsList )
        {
            if (e.InnerException != null)
            {
                errorsList.Add($"-{start} {e.InnerException.Message}");
                GetInnerExceptions(start + "-",e.InnerException, errorsList);
            }
        }

        protected JsonNoticeViewModel<T> GetModel(string error)
        {
            return new JsonNoticeViewModel<T>()
            {
                Result = "Failure",
                ValidationResults = new List<string>(new[] { error })
            };
        }

        protected JsonNoticeViewModel<int> GetModel(int id)
        {
            return new JsonNoticeViewModel<int>()
            {
                Result = "Success",
                Value = id
            };
        }

        protected JsonNoticeViewModel<int> GetModel()
        {
            return new JsonNoticeViewModel<int>()
            {
                Result = "Success",
                Value = 0
            };
        }

        protected JsonNoticeViewModel<T> GetModel(IEnumerable<ValidationResult> results)
        {
            return new JsonNoticeViewModel<T>()
            {
                Result = "Failure",
                ValidationResults =
                    results.Select(r => r.ErrorMessage).ToList()
            };
        }
        protected JsonNoticeViewModel<T> GetModel(ModelStateDictionary modelState)
        {
            return new JsonNoticeViewModel<T>
            {
                Result = "Failure",
                ValidationResults =
                    modelState.Values.Where(s => s.Errors.Any())
                        .Select(s => s.Errors[0].ErrorMessage)
                        .ToList()
            };
        }

    }
}