using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UtilitesLayer.DTOs.Teacher;
using UtilitesLayer.Services;
using UtilitesLayer.Utilities;
using WebLayer.Areas.Admin.Models;

namespace WebLayer.Areas.Admin.Controllers
{
    [Authorize(DirectoryPath.AdminRole)]
    public class UploadedFileController : BaseController
    {
        private readonly UnitOfWork db;

        public UploadedFileController(UnitOfWork db)
        {
            this.db = db;
        }
        public IActionResult Index(int page = 1, string title = null)
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" } };
            ViewData["title"] = "فایل ها";
            if (title.IsNullOrEmpty())
            {
                var da = db.UploadedFiles.GetPaggination(page, 6).Result;
                if (da.Objects.Count == 0 && page != 1)
                {
                    da = db.UploadedFiles.GetPaggination(1, 6).Result;

                }
                return View(da);
            }

            var data = db.UploadedFiles.GetPaggination(page, 6, title).Result;
            if (data.Objects.Count == 0 && page != 1)
            {
                data = db.UploadedFiles.GetPaggination(1, 6, title).Result;

            }
            return View(data);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(UploadedFileModel model)
        {
            try
            {
                if (!ModelState.IsValid) { IsRedirect(); return View(model); }
                if (db.UploadedFiles.NameExists(model.Name).Result) { ModelState.AddModelError("Name", "این نام کاربری از قبل وجود دارد"); IsRedirect(); View(model); }
                var result = await db.UploadedFiles.UploadFile(model.Name,model.File);
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
        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {

            var result = await db.UploadedFiles.DeleteFile(Id);
            await db.SaveChangesAsync();
            var a = Json(new { Status = (int)result.Status, Message = result.Message, Title = (result.Status == OperationResultStatus.Success ? "موفق" : "خطا"), IsReloadPage = true });
            return a;

        }
    }
}
