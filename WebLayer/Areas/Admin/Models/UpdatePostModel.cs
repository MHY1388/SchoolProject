using System.ComponentModel.DataAnnotations;

namespace WebLayer.Areas.Admin.Models
{
    public class UpdatePostModel
    {
        [ Display(Name = "نام"), MaxLength(200), Required(ErrorMessage = "{0} اجباری است")]
        public string Name { get; set; }
        [Display(Name = "اسلاگ"), MaxLength(200)]
        public string? Slug { get; set; }
        [ Display(Name = "توضیحات"), MaxLength(400), Required(ErrorMessage = "{0} اجباری است")]
        public string Description { get; set; }
        [ Display(Name = "محتوا"), DataType(DataType.Html), Required(ErrorMessage = "{0} اجباری است")]
        public string Content { get; set; }
        [Display(Name = "تصویر:")]
        public IFormFile? ImagePath { get; set; }

        public int CategoryID { get; set; }
        [ Display(Name = "کلید واژه ها"), Required(ErrorMessage = "{0} اجباری است")]
        public string KeyWords { get; set; }
        [Display(Name = "پست ویژه"), Required(ErrorMessage = "{0} اجباری است")]
        public bool IsSpecial { get; set; }
        public int Id { get; set; }
        [Display(Name = "زمان ایجاد")]
        public DateTime Created { get; set; }
        [Display(Name = "حذف شده")]
        public bool IsDeleted { get; set; }
    }
}
