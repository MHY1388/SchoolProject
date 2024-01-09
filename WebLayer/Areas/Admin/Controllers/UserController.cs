using DataLayer.Entities;
using Microsoft.AspNetCore.Authentication;
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
            var users = userManager.Users.AsQueryable();
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
            ViewData["header_title"] = "ثبت نام";
            ViewData["submit_title"] = "ثبت";
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link = "/admin/user", Name = "کاربران" } };
            ViewData["title"] = "ثبت نام";
            ViewData["header_title"] = "ثبت نام";
            ViewData["submit_title"] = "ثبت";
            if (!ModelState.IsValid) {this.IsRedirect();  return View(model); }
            if (!model.PhoneNumber.ValidatePhoneNumber())
            {
                IsRedirect();
                ModelState.AddModelError(String.Empty, "فرمت شماره تماس اشتباه است لطفا شماره تماس را با فرمت 09127548761 وارد نمایید");
                return View(model);
            }
            var result = await userManager.CreateAsync(new User { PhoneNumber = model.PhoneNumber.NormalizePhoneNumber(),UserName=model.UserName, FirstName = model.FirstName, LastName = model.LastName ,Number=(model.Number is not null?model.Number.Value:0)}, model.Password);
            if (result != IdentityResult.Success)
            {
                this.IsRedirect();
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                return View(model);
            }
            var user =await userManager.FindByNameAsync(model.UserName);
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

            return RedirectAndShowAlert(new UtilitesLayer.Utilities.OperationResult() { Status = UtilitesLayer.Utilities.OperationResultStatus.Success, Message = "ثبت نام با موفقیت صورت گرفت ." }, RedirectToAction("Index"));
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
            if(user == null || await userManager.IsInRoleAsync(user,DirectoryPath.UserRole))
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
            if (ReturnUrl.IsNullOrEmpty() || !Url.IsLocalUrl(ReturnUrl))
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
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            JsonResult a;
            if (user is null)
            {
                a = Json(new { Status = 404, Message = "کاربر یافت نشد", Title = "شکست", IsReloadPage = false });
            }
            else
            {
                var userrole = (await userManager.GetRolesAsync(user)).FirstOrDefault();
                if(userrole == DirectoryPath.ManagerRole)
                {
                    if (!User.IsInRole(DirectoryPath.ManagerRole))
                    {
                        return Json(new { Status = 500, Message = "شما دسترسی لازم برای حذف این کاربر را ندارید", Title = "عملیات غیر مجاز", IsReloadPage = false });
                    }
                }
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    a = Json(new { Status = 200, Message = "کاربر حذف شد", Title = "موفقیت", IsReloadPage = true });

                }
                else
                {
                    a = Json(new { Status = 404, Message = "کاربر یافت نشد", Title = "شکست", IsReloadPage = false });

                }
            }
            return a;
        }
        [HttpGet("/admin/user/updateuser/{userId}")]
        public IActionResult UpdateUser(int userId)
        {
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link = "/admin/user", Name = "کاربران" } };
            ViewData["title"] = "بروزرسانی";
            ViewData["header_title"] = "بروزرسانی";
            ViewData["submit_title"] = "بروزرسانی";
            IsRedirect();
            User? user = userManager.FindByIdAsync(userId.ToString()).Result;
            if (user == null)
            {
                return NotFound();
            }
            string? role = userManager.GetRolesAsync(user).Result.FirstOrDefault();
            UserRoles userRoles;
            if(role==DirectoryPath.UserRole)
            {
               userRoles= UserRoles.دانش_آموز;
            }
            else if (role==DirectoryPath.AdminRole)
            {
                userRoles = UserRoles.ادمین;

            }
            else if(role==DirectoryPath.ClassRole)
            {
                userRoles = UserRoles.کلاس;
            }
            else
            {
                return BadRequest();
            }
            return View(new UpdateUserModel() {Id=user.Id, FirstName=user.FirstName, LastName=user.LastName, UserName=user.UserName, Number=user.Number, PhoneNumber=user.PhoneNumber, UserRole= userRoles });
        }
        [HttpPost("/admin/user/updateuser/{userId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(int userId, UpdateUserModel user)
        {
            IsRedirect();
            ViewData["bred"] = new List<BredcompViewModel>() { new BredcompViewModel() { Link = "/admin", Name = "ادمین" }, new BredcompViewModel() { Link = "/admin/user", Name = "کاربران" } };
            ViewData["title"] = "بروزرسانی";
            ViewData["header_title"] = "بروزرسانی";
            ViewData["submit_title"] = "بروزرسانی";
            if (!ModelState.IsValid) { return View( user); }
            if (!user.PhoneNumber.ValidatePhoneNumber())
            {
                ModelState.AddModelError(String.Empty, "فرمت شماره تماس اشتباه است لطفا شماره تماس را با فرمت 09127548761 وارد نمایید");
                return View(user);
            }
            var userEntity = userManager.FindByIdAsync(userId.ToString()).Result;
            if (user is null)
            {
                return NotFound();
            }
            var userroles = await userManager.GetRolesAsync(userEntity);
            if (userroles.Contains(DirectoryPath.ManagerRole))
            {
                return BadRequest();
            }
            userEntity.Number = user.Number.Value;
            userEntity.PhoneNumber = user.PhoneNumber;
            userEntity.UserName = user.UserName;
            userEntity.FirstName = user.FirstName;
            userEntity.LastName = user.LastName;
            var hashed_password = userEntity.PasswordHash;
            if (!user.Password.IsNullOrEmpty())
            {
               hashed_password =  userManager.PasswordHasher.HashPassword(userEntity, user.Password);
            }
            IdentityResult result = userManager.UpdateAsync(userEntity).Result;
            if (result.Succeeded)
            {
                await userManager.RemoveFromRolesAsync(userEntity,userroles);
                if (user.UserRole == UserRoles.ادمین) { 
                    userEntity.ClassId = null;
                    await userManager.UpdateAsync(userEntity);
                    await userManager.AddToRoleAsync(userEntity, DirectoryPath.AdminRole);
                }
                else if (user.UserRole == UserRoles.کلاس)
                {
                    await userManager.AddToRoleAsync(userEntity, DirectoryPath.ClassRole);
                }
                else
                {
                    await userManager.AddToRoleAsync(userEntity, DirectoryPath.UserRole);
                }

                return RedirectAndShowAlert(OperationResult.Success(),RedirectToAction("Index"));
            }
            else
            {
                return RedirectAndShowAlert(OperationResult.Error(), RedirectToAction("Index"));
            }
        }

    }
}
