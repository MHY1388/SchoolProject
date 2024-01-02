using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UtilitesLayer.Services;
using UtilitesLayer.Utilities;
using WebLayer.Areas.Admin.Models;

namespace WebLayer.Areas.Admin.Controllers
{
    [Authorize(Policy =DirectoryPath.AdminRole)]
    public class ManagePresenceController : BaseController
    {
        private readonly UnitOfWork db;

        public ManagePresenceController(UnitOfWork db)
        {
            this.db = db;
        }
        public IActionResult Index(int page = 1)
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" } };
            ViewData["title"] = "کنترل حضور و غیاب";

            var da = db.Presences.GetPaggination(page, 10, DateTime.Now).Result;
            if (da.Objects.Count == 0 && page != 1)
            {
                da = db.Presences.GetPaggination(1, 10,DateTime.Now).Result;

            }
            return View(da);

        }
    }
}
