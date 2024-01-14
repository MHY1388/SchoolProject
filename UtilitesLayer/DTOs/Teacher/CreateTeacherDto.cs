using DataLayer.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilitesLayer.DTOs.Teacher
{
    public class CreateTeacherDto
    {
        [Required(ErrorMessage = "{0} اجباری است"), Display(Name ="نام"), MaxLength(255)]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "مدرک"), MaxLength(255)]
        public string Doc { get; set; }
        [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "توضیحات"), MaxLength(1000)]
        public string? Description { get; set; }
        [DataType(DataType.PhoneNumber), Required]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "نمایش عمومی شماره تلفن")]
        public bool PublicPhoneNumber { get; set; } = true;
        [Required(ErrorMessage = "{0} اجباری است"), Display(Name = ":تصویر")]
        public IFormFile Image { get; set; }
        [AllowNull]
        public int? LessonId { get; set; }
    }
    public class TeacherDto : BaseDto
    {
        [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "نام"), MaxLength(255)]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "مدرک"), MaxLength(255)]
        public string Doc { get; set; }
        public string? FilePath { get; set; }
        [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "توضیحات"), MaxLength(1000)]
        public string? Description { get; set; }
        [DataType(DataType.PhoneNumber), Required(ErrorMessage = "{0} اجباری است"), Display(Name = "شماره تماس")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "{0} اجباری است"), Display(Name ="نمایش عمومی شماره تلفن")]
        public bool PublicPhoneNumber { get; set; } = true;
        [AllowNull]
        public int? LessonId { get; set; }
        public Lesson? Lesson { get; set; }
    }
}
