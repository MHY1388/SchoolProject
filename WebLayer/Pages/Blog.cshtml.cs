using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UtilitesLayer.DTOs.Category;
using UtilitesLayer.DTOs.Global;
using UtilitesLayer.DTOs.Post;
using UtilitesLayer.Services;

namespace WebLayer.Pages
{
    public class BlogModel : PageModel
    {
        private readonly UnitOfWork db;
        public Paggination<PostDto> Posts { get; set; }
        public List<CategoryDto> Categories { get; set; }
        public BlogModel(UnitOfWork db)
        {
            this.db = db;
        }
        [FromQuery(Name = "page")]
        public int Page { get; set; } = 1;
        public void OnGet( string category = null, string name = null)
        {
            if (Page == 0)
            {
                Page = 1;
            }
            Categories = db.Categories.GetCategories(a => !a.IsDeleted);
            if(category!=null && name!=null)
            {
                ViewData["search-message"] = $"جستجو برای {category} و {name}";
                Posts = db.Posts.GetPaggination(Page, 9,name,name,category);
                if(Posts.Objects.Count == 0 && Page != 1)
                {
                    Posts = db.Posts.GetPaggination(1, 9, name, name, category);
                }
            }else if(category!=null && name==null)
            {
                ViewData["search-message"] = $"جستجو برای {category}";
                Posts = db.Posts.GetPaggination(Page, 9,category);
                if(Posts.Objects.Count == 0 && Page != 1)
                {
                    Posts = db.Posts.GetPaggination(1, 9, category);
                }
            }else if(category==null && name!=null)
            {
                ViewData["search-message"] = $"جستجو برای  {name}";
                Posts = db.Posts.GetPaggination(Page, 9,name, name);
                if(Posts.Objects.Count == 0 && Page != 1)
                {
                    Posts = db.Posts.GetPaggination(1, 9, name, name);
                }
            }else if (category == null && name == null)
                {

                Posts = db.Posts.GetPaggination(Page, 9);
                    if (Posts.Objects.Count == 0 && Page != 1)
                    {
                        Posts = db.Posts.GetPaggination(1, 9);
                    }
                }
        }
    }
}
