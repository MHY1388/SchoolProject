using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UtilitesLayer.Utilities;
using WebLayer.Areas.Admin.Models;

namespace WebLayer.Areas.Admin.Controllers
{
    [Authorize(DirectoryPath.AdminRole)]
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" } };
            ViewData["title"] = "خانه";
            return View();
        }
    }
}
