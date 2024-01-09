using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UtilitesLayer.DTOs.Teacher;
using UtilitesLayer.Services;
using UtilitesLayer.Utilities;
using WebLayer.Areas.Admin.Models;
using WebLayer.Data;

namespace WebLayer.Areas.Admin.Controllers
{
    public class TeacherController : BaseController
    {
        private readonly UnitOfWork db;
        private readonly FileManager fileManager;

        public TeacherController(UnitOfWork db, FileManager fileManager)
        {
            this.db = db;
            this.fileManager = fileManager;
        }
        // GET: TeacherController
        public IActionResult Index(int page=1, string title = null)
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" } };
            ViewData["title"] = "معلمان";
            if (title.IsNullOrEmpty())
            {
                var da = db.Teachers.GetPaggination(page, 6).Result;
                if (da.Objects.Count == 0 && page != 1)
                {
                    da = db.Teachers.GetPaggination(1, 6).Result;

                }
                return View(da);
            }

            var data = db.Teachers.GetPaggination(page, 6, title).Result;
            if (data.Objects.Count == 0 && page != 1)
            {
                data = db.Teachers.GetPaggination(1, 6, title).Result;

            }
            return View(data);
        }

        // GET: TeacherController/Add
        public IActionResult Add()
        {
            return View();
        }

        // POST: TeacherController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CreateTeacherDto model)
        {
            try
            {
                if (!ModelState.IsValid) { IsRedirect();return View(model); }
                if(!model.PhoneNumber.ValidatePhoneNumber())
                {
                    IsRedirect();
                    ModelState.AddModelError(String.Empty, "فرمت شماره تماس اشتباه است لطفا شماره تماس را با فرمت 09127548761 وارد نمایید");
                    return View(model);
                }
                var result =  await db.Teachers.CreateTecher(model);
                if (result.Status  == OperationResultStatus.Success)
                {
                    await db.SaveChangesAsync();
                     return RedirectAndShowAlert(OperationResult.Success(), RedirectToAction(nameof(Index)));
                }
                else
                {
                    return RedirectAndShowAlert(OperationResult.Error(), RedirectToAction(nameof(Index)));
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "عملیات شکست خورد");
                return View(model);
            }
        }

        // GET: TeacherController/Edit/5
        public IActionResult Update(int id)
        {
            TeacherDto data = db.Teachers.GetTecher(id).Result;
            if (data == null) { return NotFound(); }
            IsRedirect();
            var result = new UpdateTeacherModel() { Id=data.Id, Name= data.Name , Doc= data.Doc, Description =data.Description, PhoneNumber=data.PhoneNumber, PublicPhoneNumber=data.PublicPhoneNumber };
            return View(result);
        }

        // POST: TeacherController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdateTeacherModel model)
        {
            try
            {
                if (!ModelState.IsValid) { IsRedirect(); return View(model); }
                if (!model.PhoneNumber.ValidatePhoneNumber())
                {
                    IsRedirect();
                    ModelState.AddModelError(String.Empty, "فرمت شماره تماس اشتباه است لطفا شماره تماس را با فرمت 09127548761 وارد نمایید");
                    return View(model);
                }
                string imagename;
                // TODO:save image
                var OrginalTeacher = await db.Teachers.GetTecher(id);
                if(model.Image is null)
                {
                    imagename = OrginalTeacher.FilePath;
                }
                else
                {
                    await fileManager.DeleteFile(OrginalTeacher.FilePath, DirectoryPath.TeacherImages, DirectoryPath.BucketName);
                    imagename = await fileManager.SaveFile(model.Image,DirectoryPath.TeacherImages, DirectoryPath.BucketName) ;
                }
                model.FilePath = imagename;
                var result = await db.Teachers.UpdateTecher(model);
                if (result.Status == OperationResultStatus.Success)
                {
                    await db.SaveChangesAsync();
                    return RedirectAndShowAlert(OperationResult.Success(), RedirectToAction(nameof(Index)));
                }
                else
                {
                    return RedirectAndShowAlert(OperationResult.Error(), RedirectToAction(nameof(Index)));
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "عملیات شکست خورد");
                return View(model);
            }
        }

        // POST: TeacherController/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {

            var result = await db.Teachers.DeleteTeacher(Id);
            await db.SaveChangesAsync();
            var a = Json(new { Status = (int)result.Status, Message = result.Message, Title = (result.Status == OperationResultStatus.Success ? "موفق" : "خطا"), IsReloadPage = true });
            return a;

        }
    }
}
