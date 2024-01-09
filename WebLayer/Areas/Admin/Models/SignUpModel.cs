using System.ComponentModel.DataAnnotations;

namespace WebLayer.Areas.Admin.Models
{
    public class SignUpModel
    {
        [Display(Name = "نام کاربری"),Required(ErrorMessage = "{0} اجباری است")]
        public string UserName { get; set; }
        [Display(Name = "آمار")]
        public int? Number { get; set; }
        [Display(Name = "شماره تلفن"), DataType(DataType.PhoneNumber),Required(ErrorMessage = "{0} اجباری است")]
        public string PhoneNumber { get; set; }
        [Display(Name = "نام"), Required(ErrorMessage = "{0} اجباری است")]
        public string FirstName { get; set; }
        [Display(Name = "نام خانوادگی"), Required(ErrorMessage = "{0} اجباری است")]
        public string LastName { get; set; }

        [Display(Name = "رمز"), DataType(dataType:DataType.Password), Required(ErrorMessage = "{0} اجباری است")]
        public string Password { get; set; }
        [Display(Name = "تکرار رمز"), DataType(dataType:DataType.Password), Compare(nameof(Password), ErrorMessage ="رمز ها یکسان نیستند"), Required(ErrorMessage = "{0} اجباری است")]
        public string RepetitionPassword { get; set; }
        [Display(Name ="نوع")]
        public UserRoles UserRole { get; set; }
    }
    public enum UserRoles {
        ادمین,کلاس, دانش_آموز
    }
}
