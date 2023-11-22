using System.ComponentModel.DataAnnotations;

namespace WebLayer.Areas.Admin.Models
{
    public class UpdatePostModel
    {
        [Required, Display(Name = "نام"), MaxLength(200)]
        public string Name { get; set; }
        [Required, Display(Name = "اسلاگ"), MaxLength(200)]
        public string Slug { get; set; }
        [Required, Display(Name = "توضیحات"), MaxLength(400)]
        public string Description { get; set; }
        [Required, Display(Name = "محتوا"), DataType(DataType.Html)]
        public string Content { get; set; }
        [ Display(Name = "تصویر:")]
        public IFormFile? ImagePath { get; set; }

        public int CategoryID { get; set; }
        [Required, Display(Name = "بازدید ها")]
        public int Visit { get; set; }
        [Required, Display(Name = "کلید واژه ها")]
        public string KeyWords { get; set; }
        [Required, Display(Name = "پست ویژه")]
        public bool IsSpecial { get; set; }
        public int Id { get; set; }
        [Display(Name = "زمان ایجاد")]
        public DateTime Created { get; set; }
        [Display(Name = "حذف شده")]
        public bool IsDeleted { get; set; }
    }
}
