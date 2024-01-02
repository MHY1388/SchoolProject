using DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UtilitesLayer.Services;
using UtilitesLayer.Utilities;
using Microsoft.AspNet.Identity;
using UtilitesLayer.DTOs.Global;
using UtilitesLayer.DTOs.Day;
using System.Globalization;
using Microsoft.IdentityModel.Tokens;
using WebLayer.Areas.Admin.Models;
using UtilitesLayer.DTOs.Presence;
using UtilitesLayer.DTOs.Section;
using Newtonsoft.Json;

namespace WebLayer.Areas.Admin.Controllers
{
    [Authorize(Roles = DirectoryPath.ClassRole+","+DirectoryPath.AdminRole+","+DirectoryPath.ManagerRole)]
    public class PresenceController : BaseController
    {
        private readonly UnitOfWork db;
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> userManager;

        public PresenceController(UnitOfWork db, Microsoft.AspNetCore.Identity.UserManager<User> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }
        #region Days
        public IActionResult Index(int page = 1, int year = 0, int month = 0, int day=0, bool after= false, int? classId= 0)
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" } };
            ViewData["title"] = "روز ها";
            int? classid;
            if (User.IsInRole(DirectoryPath.ClassRole))
            {
                classid = userManager.FindByIdAsync(User.Identity.GetUserId()).Result.ClassId.Value;
                if(classid ==0)
                {
                    return RedirectAndShowAlert(new OperationResult() { Status = OperationResultStatus.NotFound, Message="کلاس شما یافت نشد" }, RedirectToPage("Index"));
                }
                //db.Days.AddDay(classid.Value).Wait();
                //db.SaveChanges();
            }
            else
            {
                if (classId.Value == 0||classId is null)
                {
                    return BadRequest();
                }
                classid=classId.Value;
            }
            ViewData["classId"] = classid;
            Paggination<DayDto>? data;
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
                data = db.Days.GetPaggination(page, pageSize: 10, (int)classid, day, month, year, is_bigger: after).Result;
                if (data.Objects.Count == 0)
                {
                    data = db.Days.GetPaggination(1, pageSize: 10, (int)classid, day, month, year, is_bigger: after).Result;
                }
            }
            else
            {

                data = db.Days.GetPaggination(page, pageSize: 10, (int)classid.Value).Result;
                if (data.Objects.Count == 0)
                {
                    data = db.Days.GetPaggination(1, pageSize: 10, (int)classid.Value).Result;

                }
            }
            return View(data);
        }
        public  IActionResult AddDay(int classId)
        {
            if (!db.Days.DayExsists(DateTime.Now, classId).Result)
            {
               db.Days.AddDay(classId).Wait();
               db.SaveChanges();
                return RedirectAndShowAlert(new OperationResult() { Status = OperationResultStatus.Success, Message = "روز جدید با موفقیت ایجاد" }, RedirectToAction("Index", new {classId}));
            }
            return RedirectAndShowAlert(new OperationResult() { Status = OperationResultStatus.Error, Message = "امروز از قبل ایجاد شد" }, RedirectToAction("Index", new { classId }));

        }
        #endregion
        #region Section
        public IActionResult SetSection(int sectionId,int dayId, int classid)
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link = "/admin/Presence?classId="+classid, Name = "روز ها" }, new BredcompViewModel() { Link = "/admin/Presence/SectionIndex?classId="+classid.ToString()+"&dayId="+dayId.ToString(), Name = "زنگ ها" } };
            ViewData["title"] = "حضور و غیاب";
            ViewData["sectionId"] = sectionId;
            ViewData["dayId"] = dayId;
            ViewData["classId"] = classid;
            List<PresenceDto> data = db.Presences.GetPresences(sectionId).Result;
            if (data.Count == 0)
            {
                if (classid == 0)
                {
                    return BadRequest();
                }
                SetPresences(classid, sectionId).Wait();
                data = db.Presences.GetPresences(sectionId).Result;
            }
            return View(data);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> SetSectionA(string sectionId,int dayId, int classId)
        {
            if (classId == 0)
            {
                return BadRequest();
            }
            var data = JsonConvert.DeserializeObject<List<PresenceModel>>(Request.Form["result"]);
            foreach(var item in data)
            {
                db.Presences.UpdatePresence(item.Id, item.Presence);
            }
            await db.SaveChangesAsync();
            return RedirectAndShowAlert(OperationResult.Success(), RedirectToAction("SectionIndex", new {dayId,classId}));
        }

        private async Task SetPresences(int classId, int sectionId)
        {
            var users = userManager.Users.Where(a => a.ClassId == classId).ToList();
            foreach (var item in users)
            {
                if(await userManager.IsInRoleAsync(item, DirectoryPath.UserRole))
                {
                     await db.Presences.CreatePresence(new CreatePresenceDto() { IsPresence = true , SectionID= sectionId, StudentID= item.Id});
                }
            }
            await db.SaveChangesAsync();
        }
        public IActionResult SectionIndex(int dayId, int classId, string title = null, int page=1)
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" } , new BredcompViewModel() { Link = "/admin/Presence?classId=" + classId, Name = "روز ها" } };
            ViewData["title"] = "زنگ ها";
            ViewData["DayID"] = dayId;
            if (classId == 0)
            {
                return BadRequest();
            }
            ViewData["ClassId"] = classId;
            if (title.IsNullOrEmpty())
            {
                var da = db.Sections.GetPaggination(dayId,page, 8).Result;
                if (da.Objects.Count == 0 && page != 1)
                {
                    da = db.Sections.GetPaggination(dayId, 1, 6).Result;

                }
                return View(da);
            }

            var data = db.Sections.GetPaggination(dayId, page, 6, title).Result;
            if (data.Objects.Count == 0 && page != 1)
            {
                data = db.Sections.GetPaggination(dayId, 1, 6, title).Result;

            }
            return View(data);

        }

        public IActionResult AddSection(int dayId, int classId)
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link = "/admin/Presence?classId=" + classId, Name = "روز ها" }, new BredcompViewModel() { Link = "/admin/Presence/SectionIndex?classId="+classId.ToString()+"&"+"dayId="+dayId.ToString(), Name = "زنگ ها" } };
            ViewData["title"] = "افزودن";
            var model = new CreateSectionDto() { DayId=dayId};
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSection(CreateSectionDto model, int classId)
        {
            ViewData["classId"] = classId;
            if(!ModelState.IsValid)
            {
                IsRedirect();
                return View(model);
            }
            if(await db.Sections.NameExsists(model.Name, model.DayId))
            {
                IsRedirect() ;
                ModelState.AddModelError(string.Empty, "این زنگ از قبل وجود دارد");
                return View(model);
            }
            await db.Sections.CreateSection(model);
            await db.SaveChangesAsync();
            return RedirectAndShowAlert(OperationResult.Success(), RedirectToAction("SectionIndex", new { dayId = model.DayId, classId }));
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSection(int sectionId, int dayId)
        {
           
            var result = await db.Sections.DeleteSection(sectionId);
            await db.SaveChangesAsync();
            var a = Json(new { Status = (int)result.Status, Title = (result.Status == OperationResultStatus.Success ? "موفق" : "خطا"), IsReloadPage = true });
            return a;
        }
        
        public IActionResult UpdateSection(int sectionId, int dayId, int classId)
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link = "/admin/Presence?classId=" + classId, Name = "روز ها" }, new BredcompViewModel() { Link = "/admin/Presence/SectionIndex", Name = "زنگ ها" } };
            ViewData["title"] = "بروزرسانی";
            this.IsRedirect();
            var data = db.Sections.GetSection(sectionId).Result;
            var model = new UpdateSection() { Id= sectionId , Description=data.Description, Name=data.Name};
            return View(model);
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSection(UpdateSection model, int dayId)
        {
            this.IsRedirect();

            if (!ModelState.IsValid) return View(model);
             db.Sections.UpdateSection(new SectionDto() { Id=model.Id, Description=model.Description, Name=model.Name, DayId=dayId}).Wait();
            await db.SaveChangesAsync();
            return RedirectAndShowAlert(OperationResult.Success(), RedirectToAction("SectionIndex", new { dayId }));
        }
        #endregion
    }
}
