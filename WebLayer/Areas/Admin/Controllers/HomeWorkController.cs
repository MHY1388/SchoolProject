using DataLayer.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using UtilitesLayer.DTOs.Global;
using UtilitesLayer.DTOs.HomeWork;
using UtilitesLayer.Services;
using UtilitesLayer.Utilities;
using WebLayer.Areas.Admin.Models;

namespace WebLayer.Areas.Admin.Controllers
{
    [Authorize]
    public class HomeWorkController : BaseController
    {
        private readonly UnitOfWork db;
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> userManager;

        public HomeWorkController(UnitOfWork db, Microsoft.AspNetCore.Identity.UserManager<User> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }
        public IActionResult Index(int page = 1, int year = 0, int month = 0, int day = 0, bool after = false, int? classId = 0, string title = null, HomeWorkType? homeWorkType = null)
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" } };
            ViewData["title"] = "تکالیف";
            int? classid;
            if (User.IsInRole(DirectoryPath.ClassRole)|| User.IsInRole(DirectoryPath.UserRole))
            {
                classid = userManager.FindByIdAsync(User.Identity.GetUserId()).Result.ClassId.Value;
                if (classid == 0)
                {
                    return RedirectAndShowAlert(new OperationResult() { Status = OperationResultStatus.NotFound, Message = "تکالیف شما یافت نشد" }, RedirectToPage("Index"));
                }
            }
            else
            {
                if (classId.Value == 0 || classId is null)
                {
                    return NotFound();
                }
                classid = classId.Value;
            }
            ViewData["classId"] = classid;
            Paggination<HomeWorkDto>? data;
            if (year != 0 || month != 0 || day != 0)
            {
                PersianCalendar PersianCalendar1 = new PersianCalendar();
                var now_time = DateTime.Now;
                if (year == 0)
                {
                    year = PersianCalendar1.GetYear(now_time);
                }
                if (month == 0)
                {
                    month = PersianCalendar1.GetMonth(now_time);
                }
                if (day == 0)
                {
                    day = 1;
                }
                data = db.HomeWorks.GetPaggination(page, pageSize: 10, (int)classid, title, homeWorkType, day, month, year, is_bigger: after).Result;
                if (data.Objects.Count == 0)
                {
                    data = db.HomeWorks.GetPaggination(1, pageSize: 10, (int)classid, title, homeWorkType, day, month, year, is_bigger: after).Result;
                }
            }
            else
            {
                if (title is null && homeWorkType is null)
                {

                    data = db.HomeWorks.GetPaggination(page, pageSize: 10, (int)classid.Value).Result;
                    if (data.Objects.Count == 0)
                    {
                        data = db.HomeWorks.GetPaggination(1, pageSize: 10, (int)classid.Value).Result;

                    }
                }
                {
                    data = db.HomeWorks.GetPaggination(page, pageSize: 10, (int)classid.Value,title,homeWorkType).Result;
                    if (data.Objects.Count == 0)
                    {
                        data = db.HomeWorks.GetPaggination(1, pageSize: 10, (int)classid.Value,title,homeWorkType).Result;

                    }
                }
            }
            return View(data);
        }
        [Authorize(Roles = DirectoryPath.ClassRole + "," + DirectoryPath.AdminRole + "," + DirectoryPath.ManagerRole)]
        public IActionResult Add(int classId)
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link = Url.Action("Index", "HomeWork", new {classId=classId}), Name = "تکالیف" } };
            ViewData["title"] = "افزودن";
            var data = db.Lessons.GetAll().Result;
            ViewData["Lessons"] = data;
            return View();
        }
        [Authorize(Roles = DirectoryPath.ClassRole + "," + DirectoryPath.AdminRole + "," + DirectoryPath.ManagerRole)]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add( CreateHomeWorkDto model,int? classId= null)
        {
            int classid;
            if (User.IsInRole(DirectoryPath.ClassRole))
            {
                classid = userManager.FindByIdAsync(User.Identity.GetUserId()).Result.ClassId.Value;
                if (classid == 0)
                {
                    return RedirectAndShowAlert(new OperationResult() { Status = OperationResultStatus.NotFound, Message = "تکالیف شما یافت نشد" }, RedirectToPage("Index"));
                }
            }
            else
            {
                if (classId.Value == 0 || classId is null)
                {
                    return NotFound();
                }
                classid = classId.Value;
            }
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link = Url.Action("Index", "HomeWork", new { classId = classid }), Name = "تکالیف" } };
            ViewData["title"] = "افزودن";
            var data = db.Lessons.GetAll().Result;
            ViewData["Lessons"] = data;
            var classdata = await db.Classes.GetClass(classid);
            if (classdata is null)
                return NotFound();
            if (!ModelState.IsValid)
            {
                IsRedirect();
                return View(model);
            }
            model.ClassId = classid;
            var result = await db.HomeWorks.Create(model);
            if (result.Status == OperationResultStatus.Success)
            {
                await db.SaveChangesAsync();
                return RedirectAndShowAlert(OperationResult.Success(), RedirectToAction("Index", new {classid}));
            }
            return RedirectAndShowAlert(OperationResult.Error(), RedirectToAction("Index", new { classid }));

        }
        [HttpPost]
        [Authorize(Roles = DirectoryPath.ClassRole + "," + DirectoryPath.AdminRole + "," + DirectoryPath.ManagerRole)]
        public async Task<IActionResult> Delete(int id)
        {

            var result =await db.HomeWorks.Delete(id);
            await db.SaveChangesAsync();
            var a = Json(new { Status = (int)result.Status, Message = result.Message, Title = (result.Status == OperationResultStatus.Success ? "موفق" : "خطا"), IsReloadPage = true });
            return a;
        }
        [Authorize(Roles = DirectoryPath.ClassRole + "," + DirectoryPath.AdminRole + "," + DirectoryPath.ManagerRole)]
        public IActionResult Update(int id, int? classId = null)
        {
            IsRedirect();
            int classid;
            if (User.IsInRole(DirectoryPath.ClassRole))
            {
                classid = userManager.FindByIdAsync(User.Identity.GetUserId()).Result.ClassId.Value;
                if (classid == 0)
                {
                    return RedirectAndShowAlert(new OperationResult() { Status = OperationResultStatus.NotFound, Message = "تکالیف شما یافت نشد" }, RedirectToPage("Index"));
                }
            }
            else
            {
                if (classId.Value == 0 || classId is null)
                {
                    return NotFound();
                }
                classid = classId.Value;
            }
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link = Url.Action("Index", "HomeWork", new { classId = classid }), Name = "تکالیف" } };
            ViewData["title"] = "بروزرسانی";
            var data = db.HomeWorks.Get(id).Result;
            if (data.ClassId != classid)
            {
                return BadRequest();
            }
            var datal = db.Lessons.GetAll().Result;
            ViewData["Lessons"] = datal;
            return View(data);
        }
        [Authorize(Roles = DirectoryPath.ClassRole + "," + DirectoryPath.AdminRole + "," + DirectoryPath.ManagerRole)]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(HomeWorkDto model, int id, int? classId = null)
        {
            IsRedirect();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            int classid;
            if (User.IsInRole(DirectoryPath.ClassRole))
            {
                classid = userManager.FindByIdAsync(User.Identity.GetUserId()).Result.ClassId.Value;
                if (classid == 0)
                {
                    return RedirectAndShowAlert(new OperationResult() { Status = OperationResultStatus.NotFound, Message = "تکالیف شما یافت نشد" }, RedirectToPage("Index"));
                }
            }
            else
            {
                if (classId.Value == 0 || classId is null)
                {
                    return NotFound();
                }
                classid = classId.Value;
            }
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link = Url.Action("Index", "HomeWork", new { classId = classid }), Name = "تکالیف" } };
            ViewData["title"] = "بروزرسانی";
            var result = await db.HomeWorks.Update(model);
            if(result.Status == OperationResultStatus.Success)
            {
                await db.SaveChangesAsync();
                return RedirectAndShowAlert(OperationResult.Success(), RedirectToAction("Index", new { classId = classid }));
            }
            return RedirectAndShowAlert(OperationResult.Error(), RedirectToAction("Index", new { classId = classid }));


        }

    }
}
