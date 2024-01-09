using System.ComponentModel.DataAnnotations;

namespace UtilitesLayer.DTOs.Post;

public class PostDto:BaseDto
{
    [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "نام"), MaxLength(200)]
    public string Name { get; set; }
    [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "اسلاگ"), MaxLength(200)]
    public string Slug { get; set; }
    [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "توضیحات"), MaxLength(400)]
    public string Description { get; set; }
    [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "محتوا"), DataType(DataType.Html)]
    public string Content { get; set; }
    public string ImagePath { get; set; }

    public int CategoryID { get; set; }
    public DataLayer.Entities.Category Category { get; set; }
    [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "بازدید ها")]
    public int Visit { get; set; }
    [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "کلید واژه ها")]
    public string KeyWords { get; set; }
    [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "پست ویژه")]
    public bool IsSpecial { get; set; }
    [Display(Name = "حذف شده")]
    public bool IsDeleted { get; set; }
}