using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UtilitesLayer.DTOs.Post;
using UtilitesLayer.Services;
using UtilitesLayer.Utilities;
using UtilitesLayer.Utilities.ali;
using WebLayer.Areas.Admin.Models;

namespace WebLayer.Areas.Admin.Controllers
{
    [Authorize(DirectoryPath.AdminRole)]
    public class PostController : BaseController
    {
        private readonly UnitOfWork _postServices;
        private readonly FileManager fileManager;
        private readonly CloudTool cloudTool;

        public PostController(UnitOfWork postServices, FileManager fileManager, CloudTool cloudTool)
        {
            _postServices = postServices;
            this.fileManager = fileManager;
            this.cloudTool = cloudTool;
        }
        public IActionResult Index(string title = null, int page = 1)
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" } };
            ViewData["title"] = "اخبار";
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

        public async Task<IActionResult> Add()
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link = Url.Action("Index", "Post"), Name = "اخبار" } };
            ViewData["title"] = "افزودن";
            var select = _postServices.Categories.GetCategories().Where(a => !a.IsDeleted)
                .Select(a => new CategoryOption() { Id = a.Id, Name = a.Name });
            ViewData["categories"] = select.ToList();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CreatePostDto model)
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link = Url.Action("Index", "Post"), Name = "اخبار" } };
            ViewData["title"] = "افزودن";
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

      
            var result = await _postServices.Posts.CreatePost(model);
            if (result.Status != OperationResultStatus.Success)
            {
                ModelState.AddModelError("", "عملیات با شکست مواجه شد");
                return View(model);
            }

            await _postServices.SaveChangesAsync();

            return RedirectToAction("Index", "Post");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = _postServices.Posts.DeletePost(id);
            await _postServices.SaveChangesAsync();
            var a = Json(new { Status = (int)result.Status, Message = result.Message, Title = (result.Status == OperationResultStatus.Success ? "موفق" : "خطا"), IsReloadPage = true });
            return a;
        }
        public IActionResult Update(int id)
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link = Url.Action("Index", "Post"), Name = "اخبار" } };
            ViewData["title"] = "بروزرسانی";
            var model = _postServices.Posts.GetPost(id);
            var select = _postServices.Categories.GetCategories().Where(a => !a.IsDeleted)
                .Select(a => new CategoryOption() { Id = a.Id, Name = a.Name });
            ViewData["categories"] = select.ToList();
            
            return View(new UpdatePostModel(){Id = model.Id, Name = model.Name, IsDeleted = model.IsDeleted, IsSpecial = model.IsSpecial, Content = model.Content , CategoryID = model.CategoryID,Slug = model.Slug, Description = model.Description, KeyWords = model.KeyWords});
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdatePostModel model)
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link = Url.Action("Index", "Post"), Name = "اخبار" } };
            ViewData["title"] = "بروزرسانی";
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
                await fileManager.DeleteFile(postDto.ImagePath, DirectoryPath.MediaImages, DirectoryPath.BucketName);
                filepath = await fileManager.SaveFile(model.ImagePath, DirectoryPath.MediaImages, DirectoryPath.BucketName);
            }
            var result = _postServices.Posts.UpdatePost(new PostDto(){Id = model.Id, CategoryID = model.CategoryID,Content = model.Content, Description = model.Description,ImagePath = filepath, IsDeleted = model.IsDeleted, IsSpecial = model.IsSpecial, KeyWords = model.KeyWords, Name = model.Name, Slug = model.Slug});
            await _postServices.SaveChangesAsync();
            return RedirectAndShowAlert(result, RedirectToAction("Index","Post"));
        }
        public async Task<IActionResult> Upload(IFormFile upload)
        {
            string var = await fileManager.SaveFile(upload, DirectoryPath.MediaImagesContent, DirectoryPath.BucketName, DateTime.Now.AddYears(3));
            return Json(new { uploaded = true, url = cloudTool.GeneratePreSignedUrl(var, DirectoryPath.BucketName, DateTime.Now.AddYears(3)) });
        }
    }
}
