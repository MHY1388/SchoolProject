using DataLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UtilitesLayer.Services;
using UtilitesLayer.Utilities;
using WebLayer.Areas.Admin.Models;

namespace WebLayer.Areas.Admin.Controllers
{
    [Authorize(DirectoryPath.AdminRole)]
    public class UserController : BaseController
    {
        private readonly RoleManager<Role> roleManager;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly UnitOfWork db;

        public UserController(RoleManager<Role> roleManager, UserManager<User> userManager, SignInManager<User> signInManager, UnitOfWork db)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.db = db;
        }
        public IActionResult Index(string title = null, int page = 1)
        
        {
            //await roleManager.CreateAsync(new Role() { Name = "Admin" });
            //await roleManager.CreateAsync(new Role() { Name = "Class" });
            //await roleManager.CreateAsync(new Role() { Name = "User" });
            var users = userManager.Users.ToList();
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" } };
            ViewData["title"] = "کاربران";
            if (title.IsNullOrEmpty())
            {
                var da = db.Users.GetPaggination(users, page, 10).Result;
                if (da.Objects.Count == 0 && page != 1)
                {
                    da = db.Users.GetPaggination(users, 1, 10).Result;

                }
                return View("Index", da);
            }

            var data = db.Users.GetPaggination(users, page, 10, title, title,title).Result;
            if (data.Objects.Count == 0 && page != 1)
            {
                data = db.Users.GetPaggination(users, 1, 10, title, title, title).Result;

            }
            return View("Index", data);
        }
        public IActionResult SignUp()
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link ="/admin/user", Name="کاربران"} };
            ViewData["title"] = "ثبت نام";
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link = "/admin/user", Name = "کاربران" } };
            ViewData["title"] = "ثبت نام";
            if (!ModelState.IsValid) {this.IsRedirect();  return View(model); }
            var result = await userManager.CreateAsync(new User { PhoneNumber = model.PhoneNumber.NormalizePhoneNumber(),UserName=model.UserName, FirstName = model.FirstName, LastName = model.LastName }, model.Password);
            var user =await userManager.FindByNameAsync(model.UserName);
            if (result != IdentityResult.Success)
            {
                this.IsRedirect();
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                return View(model);
            }
            if (model.UserRole == UserRoles.ادمین)
               await userManager.AddToRoleAsync(user, DirectoryPath.AdminRole);
            else if(model.UserRole== UserRoles.کلاس)
            {
                await userManager.AddToRoleAsync(user, DirectoryPath.ClassRole);
            }
            else
            {
                await userManager.AddToRoleAsync(user, DirectoryPath.UserRole);
            }

            return RedirectAndShowAlert(new UtilitesLayer.Utilities.OperationResult() { Status = UtilitesLayer.Utilities.OperationResultStatus.Success, Message = "ثبت نام با موفقیت صورت گرفت ." }, RedirectToAction("Login"));
        }
        [AllowAnonymous]
        public IActionResult Login()
        {

            if (signInManager.IsSignedIn(User))
                return Redirect("/");
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link = "/admin/user", Name = "کاربران" } };
            ViewData["title"] = "ورود";
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model, string ReturnUrl="")
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link = "/admin/user", Name = "کاربران" } };
            ViewData["title"] = "ورود";
            if (!ModelState.IsValid) { this.IsRedirect(); return View(model); }
            User? user =await userManager.FindByNameAsync(model.UserName);
            if(user == null)
            {
                ModelState.AddModelError("", "نام کاربری یا رمز عبور اشتباه است");
                return View(model);
            }
            var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RemmeberMy, true);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "نام کاربری یا رمز عبور اشتباه است");
                return View(model);
            }else if (result.IsLockedOut)
            {
                return RedirectAndShowAlert(new UtilitesLayer.Utilities.OperationResult() { Status = UtilitesLayer.Utilities.OperationResultStatus.Error, Message = "حساب شما قفل گردیده است لطفا فردا مجددا تلاش کنید." }, Redirect("/"));
            }else if (result.IsNotAllowed)
            {
                ModelState.AddModelError("", "ورود مجاز نیست.");
                return View(model);
            }else if (result.RequiresTwoFactor)
            {
                ModelState.AddModelError("", "ورود مجاز نیست.");
                return View(model);
            }
            if (ReturnUrl.IsNullOrEmpty())
            {
                ReturnUrl = "/";
            }
            return RedirectAndShowAlert(new UtilitesLayer.Utilities.OperationResult() { Status = UtilitesLayer.Utilities.OperationResultStatus.Success, Message = "ورود با موفقیت صورت گرفت ." }, Redirect(ReturnUrl));
        }
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            JsonResult a = null;
            if (!signInManager.IsSignedIn(User))
            {
                a = Json(new { Status = 10, Message = "باید ابتدا وارد شوید", Title = "عملیات غیر مجاز", IsReloadPage = false });

            }
            else
            {

                await signInManager.SignOutAsync();
                a = Json(new { Status = 200, Message = "شما با موفقیت از حساب خود خارج شدید.", Title = "عملیات موفق", IsReloadPage = true });
            }
            return a;
        }
    }
}
