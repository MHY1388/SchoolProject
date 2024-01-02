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
            var data= new PresenceDto() { StudentID = dto.StudentID, Student = dto.Student, IsPresence = dto.IsPresence, SectionID=dto.SectionId, Id = dto.Id };
            data  = BaseMapper.BaseMap(dto, data);
            return data;
        }
        public static CustomPresenceDto MapToDto(this Presence dto, string classname, string section)
        {
            return new CustomPresenceDto() { StudentID = dto.StudentID, Student = dto.Student, IsPresence = dto.IsPresence, SectionID=dto.SectionId, Id = dto.Id ,SectionName=section, ClassName=classname};
        }
        public static Presence MapToDto(this CreatePresenceDto dto)
        {
            return new Presence() { StudentID = dto.StudentID, IsPresence = dto.IsPresence, SectionId= dto.SectionID };
        }
    }
}
