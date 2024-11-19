using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UtilitesLayer.Services;
using UtilitesLayer.Utilities;
using WebLayer.Areas.Admin.Models;

namespace WebLayer.Areas.Admin.Controllers
{
    [Authorize(DirectoryPath.AdminRole)]
    public class UploadedFileController : Controller
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
    }
}
