using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class Student:BaseEntity
    {
        [Required, MaxLength(200)]
        public string FirstName { get; set; }
        [Required, MaxLength(200)]
        public string LastName { get; set; }
        [Required]
        public int? ClassId { get; set; }
        [ForeignKey(nameof(ClassId))]
        public Class? UserClass { get; set; }
    }
}
