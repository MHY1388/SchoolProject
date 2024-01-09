using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UtilitesLayer.DTOs.Category
{
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "نام")]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "اسلاگ")]
        public string Slug { get; set; }
    }

    public class CategoryDto:BaseDto
    {
        [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "نام")]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "نام")]
        public string Slug { get; set; }
        public ICollection<DataLayer.Entities.Post>? Posts { get; set; }
        [Display(Name = "حذف شده")]
        public bool IsDeleted { get; set; }
    }
}
