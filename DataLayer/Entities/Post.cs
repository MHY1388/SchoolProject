

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities
{
    public class Post : BaseEntity
    {
        [Required, MaxLength(200)]
        public string Name { get; set; }
        [Required, MaxLength(400)]
        public string Description { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string ImagePath { get; set; }

        public int CategoryID { get; set; }
        [ForeignKey(nameof(CategoryID))]
        public Category Category { get; set; }

        public int Visit { get; set; } = 0;
        public string KeyWords { get; set; }
        public bool IsSpecial { get; set; }=false;
    }
}
