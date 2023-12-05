using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class Class:BaseEntity
    {
        [Required]
        public int Grid { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<User> Students { get; set; }
        public ICollection<Day> Days { get; set; }
    }
}
