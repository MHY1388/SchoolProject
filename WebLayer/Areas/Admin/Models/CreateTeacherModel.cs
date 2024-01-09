using System.ComponentModel.DataAnnotations;
using UtilitesLayer.DTOs.Teacher;

namespace WebLayer.Areas.Admin.Models
{
    public class UpdateTeacherModel:TeacherDto
    {
        [Display(Name = "تصویر")]
        public IFormFile? Image { get; set; }
    }
}
