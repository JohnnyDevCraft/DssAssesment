using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ToDoList.EntityFramework;
using ToDoList.Web.Extensions;
using ToDoList.Web.Models;

namespace ToDoList.Web.Controllers
{
    public class CategoriesController : Controller
    {

        private ToDoListDbContext _db { get; }

        private CategoryManager<Category> _cm { get; }

        public CategoriesController()
        {
            _db = new ToDoListDbContext();

            _cm = new CategoryManager<Category>(new CategoryStore<Category>(_db));
        }

        // GET: Categories
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAllAsync()
        {
            var results = await _cm.GetAllAsync();

            var models = results.Select(r => r.ToViewModel()).ToList();

            return Json(models, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("categories/{id:int}")]
        public async Task<JsonResult> GetAsync(int id)
        {
            var result = await _cm.GetCategoryByIdAsync(id);

            var model = result.ToViewModel();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> CreateAsync([Bind(Include = "Name, Description")]CategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new JsonNoticeViewModel<CategoryViewModel>
                {
                    Result = "Failure",
                    ValidationResults =
                        ModelState.Values.Where(s => s.Errors.Any())
                            .Select(s => s.Errors[0].ErrorMessage)
                            .ToList()
                }, JsonRequestBehavior.AllowGet);

            var obj = model.ToDataModel();

            var result = await _cm.AddAsync(obj);

            if (result.ValidationResults.HasErrors())
            {
                return Json(new JsonNoticeViewModel<CategoryViewModel>()
                {
                    Value = null,
                    Result = "Failure",
                    ValidationResults = result.ValidationResults.Select(r=>r.ErrorMessage).ToList()
                }, JsonRequestBehavior.AllowGet);
            }

            var rv = await _cm.GetAllAsync();

            return Json(new JsonNoticeViewModel<List<CategoryViewModel>>()
            {
                Result = "Success",
                Value = rv.Select(c=>c.ToViewModel()).ToList()
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateAsync([Bind(Include = "Id, Name, Description")]CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var obj = model.ToDataModel();
                var result = await _cm.Update(obj);

                if (result.HasErrors())
                {
                    return Json(new JsonNoticeViewModel<CategoryViewModel>()
                    {
                        Value = null,
                        Result = "Failure",
                        ValidationResults = result.Select(r=>r.ErrorMessage).ToList()
                    }, JsonRequestBehavior.AllowGet);
                }

                var rv = await _cm.GetCategoryByIdAsync(obj.Id);

                return Json(new JsonNoticeViewModel<CategoryViewModel>()
                {
                    Result = "Success",
                    Value = rv.ToViewModel()
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonNoticeViewModel<CategoryViewModel>
            {
                Result = "Failure",
                ValidationResults = ModelState.Values.Where(s => s.Errors.Any()).Select(s => s.Errors[0].ErrorMessage).ToList()
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("categories/delete/{id:int}")]
        public async Task<JsonResult> DeleteAsync(int id)
        {
            var model = await _cm.GetCategoryByIdAsync(id);
            if (model != null)
            {
                if (await _cm.Delete(model))
                {
                    return Json(new JsonNoticeViewModel<CategoryViewModel>
                    {
                        Result = "Success"
                    }, JsonRequestBehavior.AllowGet);

                }
            }

            return Json(new JsonNoticeViewModel<CategoryViewModel>
            {
                Result = "Failure",
                ValidationResults = new List<string>() {"Unable to delete record."}
            }, JsonRequestBehavior.AllowGet);
        }


    }
}