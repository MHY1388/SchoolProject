using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class Category:BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Slug { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
