using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities;

public class Day:BaseEntity
{
    public List<Section> Sections { get; set; }
    [Required]
    public int classId { get; set; }
    [ForeignKey(nameof(classId))]
    public Class DayClass { get; set; }
    [Required]
    public DateTime Date { get; set; }
}