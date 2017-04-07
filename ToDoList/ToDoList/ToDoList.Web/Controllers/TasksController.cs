using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using ToDoList.EntityFramework;
using ToDoList.Web.Extensions;
using ToDoList.Web.Models;
using Tasks = System.Threading.Tasks;

namespace ToDoList.Web.Controllers
{
	public class TasksController : BaseJsonController<TaskJsonViewModel>
	{
        #region Fields
        private ToDoListDbContext ToDoListDbContext { get; }

		private TaskManager<Task> TaskManager { get; }
        private CategoryManager<Category> CategoryManager { get; }

        #endregion

        #region Contructors

        public TasksController()
		{
			var toDoListDbContext = new ToDoListDbContext();
			ToDoListDbContext = toDoListDbContext;

			TaskManager = new TaskManager<Task>(new TaskStore<Task>(toDoListDbContext));
            CategoryManager = new CategoryManager<Category>(new CategoryStore<Category>(toDoListDbContext));
		}

        #endregion

        protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				ToDoListDbContext.Dispose();
			}

			base.Dispose(disposing);

			return;
		}

        #region Default Web Views
        [HttpGet]
        public ActionResult Index()
		{
			return View();
		}

		public ActionResult Create()
		{
			var viewModel = new CreateTaskViewModel();
			return View(viewModel);
		}

		[HttpPost]
		public async Tasks.Task<ActionResult> Create(CreateTaskViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var task = new Task
				{
					Name = viewModel.Name,
					Description = viewModel.Description,
					Priority = viewModel.Priority.GetValueOrDefault(),
					DueDate = viewModel.DueDate.GetValueOrDefault()
				};

				var result = await TaskManager.AddAsync(task);
				var validationResults = result.ValidationResults;

				if (!validationResults.HasErrors())
				{
					TempData["SuccessMessage"] = $"Successfully created task {task.Name} ({result.Id.GetValueOrDefault().ToString(NumberFormatInfo.InvariantInfo)}).";
					return RedirectToAction("Index");
				}

				ModelState.AddRange(validationResults);
			}
			return View(viewModel);
		}

		public async Tasks.Task<ActionResult> Delete(int id)
		{
			var task = await TaskManager.GetTaskById(id);

			if (task == null)
			{
				TempData["ErrorMessage"] = $"Failed to delete task.";
				return RedirectToAction("Index");
			}

			var success = await TaskManager.Delete(task);

			if (success)
			{
				TempData["SuccessMessage"] = $"Successfully deleted task {task.Name} ({task.Id}).";
			}
			else
			{
				TempData["ErrorMessage"] = $"Failed to delete task {task.Name} ({task.Id}).";
			}

			return RedirectToAction("Index");
		}

		public async Tasks.Task<ActionResult> Edit(int id)
		{
			var task = await TaskManager.GetTaskById(id);
			var viewTask = new TaskEditViewModel(task);
			return View(viewTask);
		}

		[HttpPost]
		public async Tasks.Task<ActionResult> Edit(TaskEditViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var task = new Task
				{
					Id = viewModel.Id,
					Name = viewModel.Name,
					Description = viewModel.Description,
					Priority = viewModel.Priority,
					DueDate = viewModel.DueDate,
					IsCompleted = viewModel.Completed,
                    CompletedAt = viewModel.CompletedAt
                    
				};

                
				var validationResults = await TaskManager.Update(task);

				if (validationResults == null)
				{
					TempData["SuccessMessage"] = $"Successfully edited task {task.Name} ({task.Id}).";
					return RedirectToAction("Index");
				}

				ModelState.AddRange(validationResults);
			}
			return View(viewModel);
		}

        #endregion

        #region Json WebViews
        [HttpGet]
	    public async Tasks.Task<JsonResult> FindAsync(int id)
	    {
	        var task = await TaskManager.GetTaskById(id);

	        task.Category = await CategoryManager.GetCategoryByIdAsync(task.CategoryId);

	        return Json(task != null ? 
                GetModel(task.ToViewModel()) : 
                GetModel("Unable to locate record."), 
                JsonRequestBehavior.AllowGet);
	    }
        [HttpGet]
        public async Tasks.Task<JsonResult> GetAllAsync()
	    {
	        var tasks = await TaskManager.GetAllAsync();

	        foreach (var t in tasks)
	        {
	            t.Category = await CategoryManager.GetCategoryByIdAsync(t.CategoryId);
	        }

	        if (tasks == null || !tasks.Any())
                return Json(GetModel("No Records Found"), JsonRequestBehavior.AllowGet);

	        var models = tasks.Select(t => t.ToViewModel()).ToList();

	        return Json(GetModels(models), JsonRequestBehavior.AllowGet);
	    }
        [HttpGet]
        public async Tasks.Task<JsonResult> SearchAsync(string id)
	    {
	        try
	        {
	            var alltasks = await TaskManager.GetAllAsync();

                foreach (var t in alltasks)
                {
                    t.Category = await CategoryManager.GetCategoryByIdAsync(t.CategoryId);
                }

                var search =
	                alltasks.Where(t => t.Name != null && t.Name.Contains(id) || t.Description != null && t.Description.Contains(id))
	                    .Select(t => t.ToViewModel())
	                    .ToList();

	            return !search.Any()? 
	                Json(GetModel("No Results"), JsonRequestBehavior.AllowGet): 
	                Json(GetModels(search), JsonRequestBehavior.AllowGet);
	        }
	        catch (Exception e)
	        {
	            return Json(GetModel(e), JsonRequestBehavior.AllowGet);
	        }
	    }
        [HttpPost]
	    public async Tasks.Task<JsonResult> CreateAsync([Bind(Include = "Name, Description, Priority, DueDate, CategoryId")]TaskJsonViewModel model)
	    {
            try
            {
                if (!ModelState.IsValid)
                    return Json(GetModel(ModelState), JsonRequestBehavior.AllowGet);

                var result = await TaskManager.AddAsync(model.ToDataModel());

                return result.ValidationResults.HasErrors() ?
                    Json(GetModel(result.ValidationResults), JsonRequestBehavior.AllowGet) :
                    Json(GetModel(result.Id.Value), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(GetModel(ex), JsonRequestBehavior.AllowGet);
            }
	    }
        [HttpPost]
	    public async Tasks.Task<JsonResult> UpdateAsync(TaskJsonViewModel model)
	    {
	        try
	        {
	            if (!ModelState.IsValid)
	                return Json(GetModel(ModelState), JsonRequestBehavior.AllowGet);

	            var task = await TaskManager.Update(model.ToDataModel());

	            return task.HasErrors() ? 
	                Json(GetModel(task), JsonRequestBehavior.AllowGet) : 
	                Json(GetModel());
	        }
	        catch (Exception e)
	        {
	            return Json(GetModel(e), JsonRequestBehavior.AllowGet);
	            throw;
	        }
	    }
        [HttpGet]
        public async Tasks.Task<JsonResult> DeleteAsync(int id)
	    {
	        try
	        {
	            if (await TaskManager.Delete(await TaskManager.GetTaskById(id)))
	            {
	                return Json(GetModel(), JsonRequestBehavior.AllowGet);
	            }

	            return Json(GetModel("Unable to delete record."), JsonRequestBehavior.AllowGet);
	        }
	        catch (Exception e)
	        {
	            return Json(GetModel(e), JsonRequestBehavior.AllowGet);
	            throw;
	        }
	    }

        #endregion


    }
}