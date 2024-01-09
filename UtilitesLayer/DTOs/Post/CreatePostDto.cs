using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace UtilitesLayer.DTOs.Post
{
    public class CreatePostDto
    {
        [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "نام"), MaxLength(200)]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "اسلاگ"), MaxLength(200)]
        public string Slug { get; set; }
        [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "توضیحات"), MaxLength(400)]
        public string Description { get; set; }
        [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "محتوا"), DataType(DataType.Html)]
        public string Content { get; set; }
        [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "تصویر")]
        public IFormFile Image { get; set; }
        [Display(Name = "دسته بندی:")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "کلید واژه ها")]
        public string KeyWords { get; set; }
        [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "پست ویژه")]
        public bool IsSpecial { get; set; }
    }
}
