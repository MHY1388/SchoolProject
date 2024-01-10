using DataLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UtilitesLayer.Services;
using UtilitesLayer.Utilities;
using WebLayer.Areas.Admin.Models;

namespace WebLayer.Areas.Admin.Controllers
{
    [Authorize(DirectoryPath.AdminRole)]

    public class LessonController : BaseController
    {
        private readonly UnitOfWork _unitOfWork;

        public LessonController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(string title = null, int page = 1)
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" } };
            ViewData["title"] = "درس ها";
            if (title.IsNullOrEmpty())
            {
                var da = _unitOfWork.Lessons.GetPaggination(10, page).Result;
                if (da.Objects.Count == 0 && page != 1)
                {
                    da = _unitOfWork.Lessons.GetPaggination(10, 1).Result;

                }
                return View(da);
            }

            var data = _unitOfWork.Lessons.GetPaggination(10,a=>a.Name.Contains(title), page).Result;
            if (data.Objects.Count == 0 && page != 1)
            {
                data = _unitOfWork.Lessons.GetPaggination(10, a => a.Name.Contains(title), 1).Result;

            }
            return View(data);
        }

        public IActionResult Add()
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link = Url.Action("Index", "Lesson"), Name = "درس ها" } };
            ViewData["title"] = "افزودن";
            return View();
        }
        [ValidateAntiForgeryToken, HttpPost]
        public async Task<IActionResult> Add(CreateLessonModel model)
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link = Url.Action("Index", "Lesson"), Name = "درس ها" } };
            ViewData["title"] = "افزودن";
            if (!ModelState.IsValid) return View(model);
            if (_unitOfWork.Lessons.NameExsists(model.Name).Result)
            {
                ModelState.AddModelError("", "این نام از قبل استفاده شده است");
                this.IsRedirect();
                return View(model);
            }

            var result = _unitOfWork.Lessons.Create(model.Name).Result;
            if (result.Status != OperationResultStatus.Success)
            {
                ModelState.AddModelError("", "عملیات با شکست مواجه شد");
                this.IsRedirect();

                return View(model);
            }
            await _unitOfWork.SaveChangesAsync();
            return RedirectAndShowAlert(new OperationResult() { Message = "عملیات با موفقیت انجام شد", Status = OperationResultStatus.Success }, RedirectToAction("Index", "Lesson"));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = _unitOfWork.Lessons.Delete(id).Result;
            await _unitOfWork.SaveChangesAsync();
            var a = Json(new { Status = (int)result.Status, Message = result.Message, Title = (result.Status == OperationResultStatus.Success ? "موفق" : "خطا"), IsReloadPage = true });
            return a;
        }

        public IActionResult Update(int id)
        {
            IsRedirect();
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link = Url.Action("Index", "Lesson"), Name = "درس ها" } };
            ViewData["title"] = "بروزرسانی";
            var model = _unitOfWork.Lessons.Get(id).Result;
            var mo = new CreateLessonModel() { Name = model.Name };
            return View(mo);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CreateLessonModel model, int id)
        {
            IsRedirect();
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link = Url.Action("Index", "Lesson"), Name = "درس ها" } };
            ViewData["title"] = "بروزرسانی";
            if (!ModelState.IsValid) return View(model);
            var entity = _unitOfWork.Lessons.Get(id).Result;

            if (entity.Name != model.Name)
            {
                if (_unitOfWork.Lessons.NameExsists(model.Name).Result)
                {
                    ModelState.AddModelError("", "این نام از قبل استفاده شده است");
                    this.IsRedirect();

                    return View(model);
                }
            }
            var result = _unitOfWork.Lessons.Update(id, model.Name).Result;
            if (result.Status != OperationResultStatus.Success)
            {
                ModelState.AddModelError("", "عملیات با شکست مواجه شد");
                this.IsRedirect();

                return View(model);
            }
            await _unitOfWork.SaveChangesAsync();
            return RedirectAndShowAlert(new OperationResult()
            { Message = "با موفقیت بروزرسانی شد", Status = OperationResultStatus.Success }, RedirectToAction("Index"));
        }
    }
}
