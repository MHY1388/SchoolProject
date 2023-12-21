using System.ComponentModel.DataAnnotations;

namespace WebLayer.Areas.Admin.Models
{
    public class UpdateSection
    {
        [Required]
        public int Id { get; set; }
        [Required, MaxLength(100), Display(Name = "نام")]
        public string Name { get; set; }
        [MaxLength(255), Display(Name = "توضیحات")]
        public string? Description { get; set; }
    }
}
