using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UtilitesLayer.Utilities;

namespace WebLayer.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize()]
    public class BaseController : Controller
    {
        protected void IsRedirect()
        {
            ViewData["isRedirect"] = true;
        }
        protected IActionResult RedirectAndShowAlert(OperationResult result, IActionResult redirectPath)
        {
            var model = JsonConvert.SerializeObject(result);
            HttpContext.Response.Cookies.Append("SystemAlert", model);
            if (result.Status != OperationResultStatus.Success)
                return View();
            return redirectPath;
        }
        protected void SuccessAlert()
        {
            var model = JsonConvert.SerializeObject(OperationResult.Success());
            HttpContext.Response.Cookies.Append("SystemAlert", model);
        }
        protected void SuccessAlert(string message)
        {
            var model = JsonConvert.SerializeObject(OperationResult.Success(message));
            HttpContext.Response.Cookies.Append("SystemAlert", model);
        }
        protected void ErrorAlert()
        {
            var model = JsonConvert.SerializeObject(OperationResult.Error());
            HttpContext.Response.Cookies.Append("SystemAlert", model);
        }
        protected void ErrorAlert(string message)
        {
            var model = JsonConvert.SerializeObject(OperationResult.Error(message));
            HttpContext.Response.Cookies.Append("SystemAlert", model);
        }

    }
}
