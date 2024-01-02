using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilitesLayer.DTOs
{
    public class BaseDto
    {
        public int Id { get; set; }
        [Display(Name = "زمان ایجاد")]
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
