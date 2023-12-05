using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities;

public class Presence:BaseEntity
{
    public int StudentID { get; set; }
    [ForeignKey(nameof(StudentID))]
    public User Student { get; set; }

    public bool IsPresence { get; set; }
}