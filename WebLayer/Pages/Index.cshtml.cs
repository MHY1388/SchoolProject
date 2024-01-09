using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UtilitesLayer.DTOs.Post;
using UtilitesLayer.DTOs.Teacher;
using UtilitesLayer.Services;

namespace WebLayer.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UnitOfWork db;
        [BindProperty]
        public List<PostDto> Posts { get; set; }
        [BindProperty]
        public List<TeacherDto> Teachers { get; set; }


        public IndexModel(UnitOfWork db)
        {
            this.db = db;
        }

        public void OnGet()
        {
            Posts=db.Posts.GetPosts();
            Teachers = db.Teachers.GetTechers().Result;
        }
    }
}