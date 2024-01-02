using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilitesLayer.DTOs.Section
{
    public class CreateSectionDto
    {
        [Required, MaxLength(100), Display(Name ="نام")]
        public string Name { get; set; }
        [MaxLength(255), Display(Name = "توضیحات")]
        public string? Description { get; set; }
        [Required]
        public int DayId { get; set; }
    }
    public class SectionDto:BaseDto
    {

        [Required, MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string? Description { get; set; }
        public List<DataLayer.Entities.Presence>? Data { get; set; }
        [Required]
        public int DayId { get; set; }
    }
}
