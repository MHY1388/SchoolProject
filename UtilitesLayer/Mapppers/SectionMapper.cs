using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilitesLayer.DTOs.Day;
using UtilitesLayer.DTOs.Section;

namespace UtilitesLayer.Mapppers
{
    public static class SectionMapper
    {
        public static Section MapToSection(this SectionDto section)
        {
            return new Section() { DayId = section.DayId, Id = section.Id,Name = section.Name, Description = section.Description, Updated= DateTime.Now };
        }
        public static SectionDto MapToDto(this Section section)
        {
            var data= new SectionDto() { DayId = section.DayId, Id = section.Id, Name = section.Name, Description = section.Description };
            data = BaseMapper.BaseMap(section, data);
            return data;
        }
        public static Section MapToDto(this CreateSectionDto section)
        {
            return new Section() { DayId = section.DayId, Name = section.Name, Description = section.Description };
        }
    }
}
