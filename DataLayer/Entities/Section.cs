using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class Section
    {
        [Required, Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string? Description { get; set; }
        public List<Presence> Data { get; set; }
        [Required]
        public int DayId { get; set; }
        [Required,ForeignKey(nameof(DayId))]
        public Day Day { get; set; }
    }
}
