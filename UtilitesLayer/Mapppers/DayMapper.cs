using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilitesLayer.DTOs.Day;

namespace UtilitesLayer.Mapppers
{
    public static class DayMapper
    {
        public static Day MapToDay(this DayDto day)
        {
            return new Day() { classId = day.classId, Id = day.Id};
        }
        public static DayDto MapToDto(this Day day)
        {
            var data =  new DayDto() { classId = day.classId, Id = day.Id };
            data = BaseMapper.BaseMap(day, data);
            return data;
        }
    }
}
