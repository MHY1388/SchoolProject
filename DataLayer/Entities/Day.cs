using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities;

public class Day:BaseEntity
{
    public ICollection<Presence> Section1 { get; set; }
    public ICollection<Presence> Section2 { get; set; }

    public ICollection<Presence> Section3 { get; set; }

    public ICollection<Presence> Section4 { get; set; }
    public int classId { get; set; }
    [ForeignKey(nameof(classId))]
    public Class DayClass { get; set; }

    public DateOnly Date { get; set; }
}