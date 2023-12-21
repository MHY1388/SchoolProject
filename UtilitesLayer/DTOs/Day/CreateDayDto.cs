using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilitesLayer.DTOs.Day
{

    public class DayDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int classId { get; set; }
        [Required]
        public DateTime Created { get; set; }
    }

}
