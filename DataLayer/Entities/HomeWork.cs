using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class HomeWork:BaseEntity
    {
        [Required, MaxLength(1000)]
        public string Description { get; set; }
        [Required, DataType(dataType:DataType.Date)]
        public DateTime LastTime { get; set; }
        [Required]
        public HomeWorkType Type { get; set; }
    }
    public class Lesson : BaseEntity
    {
        [MaxLength(100),Required]
        public string Name { get; set; }
    }
    public enum HomeWorkType
    {
        homework, test, optional, question
    }

}
