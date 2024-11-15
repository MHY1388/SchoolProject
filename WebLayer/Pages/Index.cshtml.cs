using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UtilitesLayer.DTOs.Post;
using UtilitesLayer.DTOs.Teacher;
using UtilitesLayer.Services;
using Microsoft.AspNetCore.Identity;
using UtilitesLayer.Utilities;
using WebLayer.Areas.Admin.Models;
namespace WebLayer.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UnitOfWork db;
        [BindProperty]
        public List<PostDto> Posts { get; set; }
        [BindProperty]
        public List<TeacherDto> Teachers { get; set; }

        private readonly RoleManager<Role> roleManager;
        private readonly UserManager<DataLayer.Entities.User> userManager;

        public IndexModel(UnitOfWork db, RoleManager<Role> roleManager, UserManager<DataLayer.Entities.User> userManager)
        {
            this.db = db;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public void OnGet()
        {
            if (!userManager.Users.Any())
            {
                roleManager.CreateAsync(new Role() { Id=1,Name="Admin",NormalizedName="ADMIN"}).Wait();
                roleManager.CreateAsync(new Role() { Id =2, Name = "Class", NormalizedName = "CLASS" }).Wait();
                roleManager.CreateAsync(new Role() { Id =3, Name = "User", NormalizedName = "USER" }).Wait();
                roleManager.CreateAsync(new Role() { Id = 4, Name = "Manager", NormalizedName = "MANAGER" }).Wait();

                var result = userManager.CreateAsync(new DataLayer.Entities.User { PhoneNumber = "09127536960", UserName = "Admin", FirstName = "Super", LastName = "Admin", Number =0 }, "1388W1388w@").Result;
                if (result.Succeeded)
                {
                    var user = userManager.FindByNameAsync("Admin").Result;
                    userManager.AddToRoleAsync(user, DirectoryPath.ManagerRole).Wait();
                }
                db.SaveChanges();
            }

            Posts =db.Posts.GetPosts();
            Teachers = db.Teachers.GetTechers().Result;
        }
    }
}