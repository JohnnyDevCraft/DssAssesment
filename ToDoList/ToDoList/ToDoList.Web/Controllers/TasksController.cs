using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using ToDoList.EntityFramework;
using ToDoList.Web.Models;
using Tasks = System.Threading.Tasks;

namespace ToDoList.Web.Controllers
{
	public class TasksController : Controller
	{
		private ToDoListDbContext ToDoListDbContext { get; }

		private TaskManager<Task> TaskManager { get; }

		public TasksController()
		{
			var toDoListDbContext = new ToDoListDbContext();
			ToDoListDbContext = toDoListDbContext;

			TaskManager = new TaskManager<Task>(new TaskStore<Task>(toDoListDbContext));
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				ToDoListDbContext.Dispose();
			}

			base.Dispose(disposing);

			return;
		}

		public async Tasks.Task<ActionResult> Index()
		{
			var tasks = await TaskManager.GetAllAsync();

			var tasksQuery =
				from task in tasks
				select new TaskViewModel(task);

			var viewModel = new TasksViewModel
			{
				Tasks = tasksQuery.ToList()
			};

			return View(viewModel);
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
	}
}