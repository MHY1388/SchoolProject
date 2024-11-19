using System.ComponentModel.DataAnnotations;

namespace WebLayer.Areas.Admin.Models
{
    public class UploadedFileModel
    {
        [Required,Display(Name ="فایل")]
        public IFormFile File { get; set; }
        [Required,Display(Name ="نام")]
        public string Name { get; set; }
    }
}
