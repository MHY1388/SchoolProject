using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class Student:BaseEntity
    {
        public int? ClassId { get; set; }
        [ForeignKey(nameof(ClassId))]
        public Class? UserClass { get; set; }
    }
}
