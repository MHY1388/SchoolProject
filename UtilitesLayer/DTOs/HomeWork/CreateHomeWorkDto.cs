using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UtilitesLayer.DTOs.Class;

namespace UtilitesLayer.DTOs.HomeWork
{
    public class CreateHomeWorkDto
    {
        [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "توضیحات"), MaxLength(1000)]
        public string Description { get; set; }
        [ DataType(dataType: DataType.Date)]
        [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "زمان تحویل"),UIHint("DateOnly")]
        public string LastTime { get; set; }
        [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "نوع")]
        public HomeWorkType Type { get; set; }
        [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "درس")]
        public int LessonId { get; set; }
        [Required, HiddenInput]
        public int ClassId { get; set; }
    }
    public class HomeWorkDto:BaseDto
    {
        [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "توضیحات"), MaxLength(1000)]
        public string Description { get; set; }
        [DataType(dataType: DataType.Date)]
        [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "زمان تحویل"), UIHint("DateOnly")]
        public DateTime LastTime { get; set; }
        [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "زمان تحویل"), UIHint("DateOnly")]
        public string LastTimeStr { get; set; }
        [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "نوع")]
        public HomeWorkType Type { get; set; }
        [Required(ErrorMessage = "{0} اجباری است"), Display(Name = "درس")]
        public int LessonId { get; set; }
        public Lesson? Lesson { get; set; }
        [Required, HiddenInput]
        public int ClassId { get; set; }
        public ClassDto? Class { get; set; }
    }
}
