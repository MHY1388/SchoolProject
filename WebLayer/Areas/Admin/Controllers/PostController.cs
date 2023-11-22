using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        public IActionResult Index(string title = null, int page = 1)
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" } };
            ViewData["title"] = "مقالات";
            if (title.IsNullOrEmpty())
            {
                var da = _postServices.Posts.GetPaggination(page, 6);
                if (da.Objects.Count == 0 && page != 1)
                {
                    da = _postServices.Posts.GetPaggination(1, 6);

                }
                return View(da);
            }

            var data = _postServices.Posts.GetPaggination(page, 6, title, title);
            if (data.Objects.Count == 0 && page != 1)
            {
                data = _postServices.Posts.GetPaggination(1, 6, title, title);

            }
            return View(data);
        }

        public IActionResult Add()
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link = Url.Action("Index", "Post"), Name = "مقالات" } };
            ViewData["title"] = "افزودن";
            var select = _postServices.Categories.GetCategories().Where(a => !a.IsDeleted)
                .Select(a => new CategoryOption() { Id = a.Id, Name = a.Name });
            ViewData["categories"] = select.ToList();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Add(CreatePostDto model)
        {
            var select = _postServices.Categories.GetCategories().Where(a => !a.IsDeleted)
                .Select(a => new CategoryOption() { Id = a.Id, Name = a.Name });
            if (model.Slug.IsNullOrEmpty())
                model.Slug = model.Name;
            ViewData["categories"] = select.ToList();
            if (!ModelState.IsValid)
                return View(model);
            if (_postServices.Posts.SlugExtsts(model.Slug))
            {
                ModelState.AddModelError("", "این اسلاگ قبلا استفاده شده");
                return View(model);

            }

      
            var result = _postServices.Posts.CreatePost(model);
            if (result.Status != OperationResultStatus.Success)
            {
                ModelState.AddModelError("", "عملیات با شکست مواجه شد");
                return View(model);
            }

            _postServices.SaveChanges();

            return RedirectToAction("Index", "Post");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var result = _postServices.Posts.DeletePost(id);
            _postServices.SaveChanges();
            var a = Json(new { Status = (int)result.Status, Message = result.Message, Title = (result.Status == OperationResultStatus.Success ? "موفق" : "خطا"), IsReloadPage = true });
            return a;
        }
        public IActionResult Update(int id)
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link = Url.Action("Index", "Post"), Name = "مقالات" } };
            ViewData["title"] = "بروزرسانی";
            var model = _postServices.Posts.GetPost(id);
            var select = _postServices.Categories.GetCategories().Where(a => !a.IsDeleted)
                .Select(a => new CategoryOption() { Id = a.Id, Name = a.Name });
            ViewData["categories"] = select.ToList();
            
            return View(new UpdatePostModel(){Id = model.Id, Name = model.Name, IsDeleted = model.IsDeleted, IsSpecial = model.IsSpecial, Content = model.Content , CategoryID = model.CategoryID,Slug = model.Slug, Description = model.Description, KeyWords = model.KeyWords});
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Update(UpdatePostModel model)
        {
            var select = _postServices.Categories.GetCategories().Where(a => !a.IsDeleted)
                .Select(a => new CategoryOption() { Id = a.Id, Name = a.Name });
            ViewData["categories"] = select.ToList();
            if (!ModelState.IsValid)
                return View(model);
            var postDto = _postServices.Posts.GetPost(model.Id);
            if(model.Slug != postDto.Slug)
            {
                if (_postServices.Posts.SlugExtsts(model.Slug))
                {
                    ModelState.AddModelError("", "این اسلاگ قبلا استفاده شده");
                    return View(model);
                }

            }
               
            string filepath = null;
            if (model.ImagePath is null)
            {
                filepath = postDto.ImagePath;
            }
            else
            {
                FileManager.DeleteFile(postDto.ImagePath, DirectoryPath.MediaImages);
                filepath = FileManager.SaveFile(model.ImagePath, DirectoryPath.MediaImages);
            }
            var result = _postServices.Posts.UpdatePost(new PostDto(){Id = model.Id, CategoryID = model.CategoryID,Content = model.Content, Description = model.Description,ImagePath = filepath, IsDeleted = model.IsDeleted, IsSpecial = model.IsSpecial, KeyWords = model.KeyWords, Name = model.Name, Slug = model.Slug});
            _postServices.SaveChanges();
            return RedirectAndShowAlert(result, RedirectToAction("Index","Post"));
        }
        public IActionResult Upload(IFormFile upload)
        {
            string var = FileManager.SaveFile(upload, DirectoryPath.MediaImagesContent);
            return Json(new { uploaded = true, url = DirectoryPath.MediaImagesContent.Replace("\\", "/").Replace("/wwwroot", "") + var });
        }
    }
}
