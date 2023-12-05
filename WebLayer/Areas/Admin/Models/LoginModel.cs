using System.ComponentModel.DataAnnotations;

namespace WebLayer.Areas.Admin.Models
{
    public class LoginModel
    {
        [Display(Name = "نام کاربری"), Required(ErrorMessage = "{0} اجباری است")]
        public string UserName { get; set; }
        [Display(Name = "مرا به یاد بیاور"), Required(ErrorMessage = "{0} اجباری است")]
        public bool RemmeberMy { get; set; }


        [Display(Name = "رمز"), DataType(dataType: DataType.Password), Required(ErrorMessage = "{0} اجباری است")]
        public string Password { get; set; }


    }
}
