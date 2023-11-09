

using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities
{
    public class Post : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }

        public int CategoryID { get; set; }
        [ForeignKey(nameof(CategoryID))]
        public Category Category { get; set; }

        public int Visit { get; set; } = 0;
        public List<string> KeyWords { get; set; }
    }
}
