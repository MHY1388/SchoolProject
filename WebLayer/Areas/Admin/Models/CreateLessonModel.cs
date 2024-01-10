using System.ComponentModel.DataAnnotations;

namespace WebLayer.Areas.Admin.Models
{
    public class CreateLessonModel
    {
        [Required(ErrorMessage =" اجباری است{0}"), MaxLength(1000, ErrorMessage ="طول {0} بیش از{1} است"),Display(Name ="نام") ]
        public string Name { get; set; }
    }
}
