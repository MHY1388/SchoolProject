using Microsoft.AspNetCore.Mvc;
using WebLayer.Areas.Admin.Models;

namespace WebLayer.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/", Name = "ادمین" } };
            ViewData["title"] = "خانه";
            return View();
        }
    }
}
