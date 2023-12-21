using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilitesLayer.DTOs.Presence;

namespace UtilitesLayer.Mapppers
{
    public static class PresenceMapper
    {
        public static PresenceDto MapToDto(this Presence dto)
        {
            return new PresenceDto() { StudentID = dto.StudentID, Student = dto.Student, IsPresence = dto.IsPresence, SectionID=dto.SectionId, Id = dto.Id };
        }
        public static Presence MapToDto(this CreatePresenceDto dto)
        {
            return new Presence() { StudentID = dto.StudentID, IsPresence = dto.IsPresence, SectionId= dto.SectionID };
        }
    }
}
