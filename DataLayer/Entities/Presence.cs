using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities;

public class Presence:BaseEntity
{
    [Required]
    public int StudentID { get; set; }
    [ForeignKey(nameof(StudentID))]
    public User Student { get; set; }
    [Required]
    public int SectionId { get; set; }
    [ForeignKey(nameof(SectionId))]
    public Section Section { get; set; }
    [Required]
    public bool IsPresence { get; set; }
}