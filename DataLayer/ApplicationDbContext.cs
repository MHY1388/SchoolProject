using DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebLayer.Data
{
    public class ApplicationDbContext : IdentityDbContext<User,Role,int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,IdentityRoleClaim<int>,IdentityUserToken<int>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Presence> Presences { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<Section> Sections { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            


            modelBuilder.Entity<Class>(b =>
            {
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.Students)
                    .WithOne(e => e.UserClass)
                    .HasForeignKey(ur => ur.ClassId);
            });

            
            modelBuilder.Entity<User>(b =>
            {

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRole)
                    .WithOne(e => e.ModelUser)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
                b.HasOne(e => e.UserClass)
                    .WithMany(e => e.Students)
                    .HasForeignKey(ur => ur.ClassId);
            });

            modelBuilder.Entity<Role>(b =>
            {
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.ModelRole)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });

        }
    }
}
