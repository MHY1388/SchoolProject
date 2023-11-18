using Microsoft.AspNetCore.Mvc;
using UtilitesLayer.DTOs.Post;
using UtilitesLayer.Services;
using UtilitesLayer.Utilities;
using WebLayer.Areas.Admin.Models;

namespace WebLayer.Areas.Admin.Controllers
{
    public class PostController : BaseController
    {
        private readonly UnitOfWork _postServices;

        public PostController(UnitOfWork postServices)
        {
            _postServices = postServices;
        }
        public IActionResult Index()
        {

            return View();
        }
        
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Add(CreatePostDto model)
        {
            if(!ModelState.IsValid)
                return View(model);

            var result = _postServices.Posts.CreatePost(model);
            if (result.Status != OperationResultStatus.Success)
            {
                ModelState.AddModelError("", "عملیات با شکست مواجه شد");
                return View(model);
            }

            _postServices.SaveChanges();

            return RedirectToAction("Index", "Post");
        }
    }
}
