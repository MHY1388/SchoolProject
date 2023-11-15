using Microsoft.AspNetCore.Mvc;
using WebLayer.Areas.Admin.Models;

namespace WebLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostController : Controller
    {
        public IActionResult Index()
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/", Name = "ادمین" }, new BredcompViewModel(){Link = "/post/", Name = "پست"}};
            ViewData["title"] = "خانه";
            return View();
        }
    }
}
