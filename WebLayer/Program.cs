using System.Security.Claims;
using DataLayer.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UtilitesLayer.Services;
using UtilitesLayer.Utilities;
using UtilitesLayer.Utilities.ali;
using WebLayer.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<User, Role>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireDigit = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 4;
        options.Password.RequiredUniqueChars = 2;
        options.ClaimsIdentity.EmailClaimType = ClaimTypes.Email;
        options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
        options.Lockout.AllowedForNewUsers = false;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(1);
        options.Lockout.MaxFailedAccessAttempts = 3;
       

    }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders().AddErrorDescriber<PersianIdentityErrors>();
builder.Services.TryAddScoped<UserManager<User>>();
builder.Services.TryAddScoped<SignInManager<User>>();
builder.Services.TryAddScoped<RoleManager<Role>>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Errors/500";
    options.Cookie.Name = "SchoolWebsite";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(3);
    options.LoginPath = "/Admin/User/login";
    options.LogoutPath = "/Admin/User/logout";
    // ReturnUrlParameter requires 
    //using Microsoft.AspNetCore.Authentication.Cookies;
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    options.SlidingExpiration = true;
});
//builder.Services.AddAuthorization(a => a.AddPolicy("Admin", b => b.RequireRole(new List<string>() { "Admin" })));
builder.Services.AddAuthorization(options => { options.AddPolicy(DirectoryPath.AdminRole, a => a.RequireAssertion(c=>c.User.IsInRole(DirectoryPath.AdminRole)||c.User.IsInRole(DirectoryPath.ManagerRole)));options.AddPolicy(DirectoryPath.ManagerRole, a => a.RequireRole(DirectoryPath.ManagerRole));options.AddPolicy(DirectoryPath.ClassRole, a => a.RequireRole(DirectoryPath.ClassRole)); });
builder.Services.AddScoped<UnitOfWork, UnitOfWork>();
builder.Services.AddScoped<CloudTool, CloudTool>();
builder.Services.AddScoped<FileManager, FileManager>();
builder.Services.AddRazorPages();
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseMigrationsEndPoint();
//}
//else
//{
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
//}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.Run();
