using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace UtilitesLayer.DTOs.Presence
{
    public class CreatePresenceDto
    {
        [Required]
        public int StudentID { get; set; }
        [Required]
        public bool IsPresence { get; set; }
        [Required]
        public int SectionID { get; set; }
    }
    public class PresenceDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int StudentID { get; set; }
        public User? Student { get; set; }
        [Required]

        public int SectionID { get; set; }

        [Required]
        public bool IsPresence { get; set; }
    }
}
