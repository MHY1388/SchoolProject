using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UtilitesLayer.DTOs.Category;
using UtilitesLayer.DTOs.Post;
using UtilitesLayer.Services;

namespace WebLayer.Pages
{
    public class PostModel : PageModel
    {
        private readonly UnitOfWork db;
        [BindProperty]
        public PostDto Post { get; set; }
        [BindProperty]
        public List<PostDto> RelatedPosts { get; set; }
        public List<CategoryDto> Categories { get; set; }
        [BindProperty]
        public List<PostDto> Posts { get; set; }
        public PostModel(UnitOfWork db)
        {
            this.db = db;
        }
        public IActionResult OnGet(int id, string slug)
        {
            Posts = db.Posts.GetPosts();

            Post = db.Posts.GetPost(id);
            if(Post == null)
            {
                return NotFound();
            }
            if (Post.Slug != slug)
            {
                return NotFound();
            }
            db.Posts.VisitPost(id);
            db.SaveChanges();
            RelatedPosts = Posts.Where(a=>a.CategoryID == Post.CategoryID)
                .OrderByDescending(a=>a.Visit).Take(6).ToList();
            Categories = db.Categories.GetCategories();
            ViewData["title"] = Post.Name;
            return Page();
        }
    }
}
