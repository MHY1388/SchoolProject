using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities;

public class Day:BaseEntity
{
    [Required]
    public List<Presence> Section1 { get; set; }
    [Required]
    public List<Presence> Section2 { get; set; }
    [Required]
    public List<Presence> Section3 { get; set; }
    [Required]
    public List<Presence> Section4 { get; set; }
    [Required]
    public int classId { get; set; }
    [ForeignKey(nameof(classId))]
    public Class DayClass { get; set; }
    [Required]
    public DateTime Date { get; set; }
}