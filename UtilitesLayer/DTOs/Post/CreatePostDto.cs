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
        [Required, Display(Name = "نام"), MaxLength(200)]
        public string Name { get; set; }
        [Required, Display(Name = "اسلاگ"), MaxLength(200)]
        public string Slug { get; set; }
        [Required, Display(Name = "توضیحات"), MaxLength(400)]
        public string Description { get; set; }
        [Required, Display(Name = "محتوا"), DataType(DataType.Html)]
        public string Content { get; set; }
        [Required, Display(Name = "تصویر")]
        public IFormFile Image { get; set; }
        [Display(Name = "دستبه بندی")]
        public int CategoryID { get; set; }

        [Required, Display(Name = "کلید واژه ها")]
        public string KeyWords { get; set; }
        [Required, Display(Name = "پست ویژه")]
        public bool IsSpecial { get; set; }
    }
}
