using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilitesLayer.DTOs.Day
{

    public class DayDto:BaseDto
    {
        [Required]
        public int classId { get; set; }

    }

}
