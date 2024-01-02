using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UtilitesLayer.Services;
using WebLayer.Areas.Admin.Models;
using UtilitesLayer.Utilities;
using UtilitesLayer.DTOs.Class;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using DataLayer.Entities;

namespace WebLayer.Areas.Admin.Controllers
{
    [Authorize(Policy = DirectoryPath.AdminRole)]

    public class ClassController : BaseController
    {
        private readonly UnitOfWork db;
        private readonly UserManager<User> userManager;

        public ClassController(UnitOfWork db, UserManager<User> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }
        // GET: ClassController
        public IActionResult Index(string title = null, int page = 1)
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" } };
            ViewData["title"] = "کلاس ها";
            int num = 0;
            var da = db.Classes.GetPaggination(page, 6,title).Result;
            if (da.Objects.Count == 0 && page != 1)
            {
                da = db.Classes.GetPaggination(1, 6, null).Result;

            }
            return View(da);

        }


        public IActionResult Detail(int id, string title = null, int page = 1)

        {
            //await roleManager.CreateAsync(new Role() { Name = "Admin" });
            //await roleManager.CreateAsync(new Role() { Name = "Class" });
            //await roleManager.CreateAsync(new Role() { Name = "User" });
            var users = userManager.Users.ToList();
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link = "/admin/class", Name = "کلاس ها" } };
            ViewData["title"] = "مشاهده";
            ViewData["id"] = id;

            if (title.IsNullOrEmpty())
            {
                var da = db.Users.GetPaggination( users:users,page: page,pageSize: 10,classId:id).Result;
                if (da.Objects.Count == 0 && page != 1)
                {
                    da = db.Users.GetPaggination(users, 1, 10, classId:id).Result;

                }
                return View("Detail", da);
            }

            var data = db.Users.GetPaggination(users:users,page: page,pageSize: 10,firstName: title,lastName: title,userName: title, classId:id).Result;
            if (data.Objects.Count == 0 && page != 1)
            {
                data = db.Users.GetPaggination(users,page: 1,pageSize: 10,firstName: title,lastName: title,userName: title, classId:id).Result;

            }
            return View("Detail", data);
        }

        // GET: ClassController/Create 
        public IActionResult Add()
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link="/admin/class", Name="کلاس ها"} };
            ViewData["title"] = "افزودن";
            return View();
        }

        // POST: ClassController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CreateClassDto model)
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link = "/admin/class", Name = "کلاس ها" } };
            ViewData["title"] = "افزودن";
            try
            {
                if (!ModelState.IsValid) { return View(model);
                    this.IsRedirect();
                }
                var result = await db.Classes.AddClass(model);
                if (result.Status == OperationResultStatus.Success)
                {
                    db.SaveChanges();
                    return RedirectAndShowAlert(new OperationResult() { Status = OperationResultStatus.Success }, RedirectToAction("Index"));
                }
                else
                {
                    return RedirectAndShowAlert(new OperationResult() { Status = OperationResultStatus.Error }, RedirectToAction("Index"));

                }
            }
            catch
            {
                this.IsRedirect();

                return View(model);
            }
        }

        // GET: ClassController/Edit/5
        public IActionResult Edit(int id)
        {
            ClassDto result = db.Classes.GetClass(id).Result;
            this.IsRedirect();
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link = "/admin/class", Name = "کلاس ها" } };
            ViewData["title"] = "بروزرسانی";
            return View(result);
        }
        
        // POST: ClassController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClassDto model)
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link = "/admin/class", Name = "کلاس ها" } };
            ViewData["title"] = "بروزرسانی";

            try
            {
                if (!ModelState.IsValid) { return View(model);
                    this.IsRedirect();
                }
                var result =  await db.Classes.UpdateClass(model);
                if (result.Status == OperationResultStatus.Success)
                {
                    db.SaveChanges();
                    return RedirectAndShowAlert(new OperationResult() { Status = OperationResultStatus.Success }, RedirectToAction("Index"));
                }
                else
                {
                    return RedirectAndShowAlert(new OperationResult() { Status = OperationResultStatus.Error }, RedirectToAction("Index"));

                }
            }
            catch
            {
                this.IsRedirect();
                return View(model);
            }
        }

        // POST: ClassController/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
                var result = await db.Classes.DeleteClass(id);
            db.SaveChanges();
                return Json(new { Status = (int)result.Status, Message = result.Message, Title = (result.Status == OperationResultStatus.Success ? "موفق" : "خطا"), IsReloadPage = true });

        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStudent(int id, string stId)
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link = "/admin/class", Name = "کلاس ها" } };
            ViewData["title"] = "افزودن دانش آموز";
            var class_d = await db.Classes.GetClass(id);
            if (class_d == null)
            {
                this.ErrorAlert("این کلاس وجود ندارد");
                return RedirectToAction("Detail", new { id });
            }

            var students = stId.Split(' ');
            User? studen;
            List<string> errors = new List<string>();
            foreach (var student in students)
            {
                studen = await userManager.FindByIdAsync(student);
                if (studen == null)
                    errors.Add(student);
                else if(await userManager.IsInRoleAsync(studen, DirectoryPath.AdminRole))
                {
                    errors.Add(student);
                }
                else
                {
                    studen.ClassId = id;
                    await userManager.UpdateAsync(studen);
                }
            }
            if(errors.Any()){
                if(errors.Count() == students.Count()) 
                {
                    this.ErrorAlert("همچین دانش آموزی وجود ندارد.");
                }
                else
                {

                string res = "";
                foreach (var error in errors)
                {
                    res += error+ " ";
                }
                this.ErrorAlert($"دانش آموز {res} وجود ندارد ولی بقیه افزوده شده اند.");
                }
                return RedirectToAction("Detail", new { id });

            }
            return RedirectAndShowAlert(new OperationResult() { Status=OperationResultStatus.Success}, RedirectToAction("Detail", new {id}));

        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveStudent(int id, string stId)
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link = "/admin/class", Name = "کلاس ها" } };
            ViewData["title"] = "حذف دانش آموز";
            var class_d = await db.Classes.GetClass(id);
            if (class_d == null)
            {
                this.ErrorAlert("این کلاس وجود ندارد");
                return RedirectToAction("Index");
            }

            var students = stId.Split(' ');
            User? studen;
            List<string> errors = new List<string>();
            foreach (var student in students)
            {
                studen = await userManager.FindByIdAsync(student);
                if (studen == null)
                    errors.Add(student);
                else
                {
                    studen.ClassId = null;
                    await userManager.UpdateAsync(studen);
                }
            }
            if(errors.Any()){
                string res = "";
                foreach (var error in errors)
                {
                    res += error+ " ";
                }
                this.ErrorAlert($"دانش آموز {res} وجود ندارد ولی بقیه حذف شده اند.");
                return RedirectToAction("Detail", new { id });

            }
            return RedirectAndShowAlert(new OperationResult() { Status=OperationResultStatus.Success}, RedirectToAction("Detail", new {id}));

        }
    }
}
