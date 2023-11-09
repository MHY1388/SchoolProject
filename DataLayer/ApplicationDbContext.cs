using DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebLayer.Data
{
    public class ApplicationDbContext : IdentityDbContext<User,Role, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Presence> Presences { get; set; }
        public DbSet<Day> Days { get; set; }

    }
}
