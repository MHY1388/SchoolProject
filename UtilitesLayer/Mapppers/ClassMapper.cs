using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilitesLayer.DTOs.Class;

namespace UtilitesLayer.Mapppers
{
    public static class ClassMapper
    {
        public static ClassDto MapToDto(this Class entity)
        {
            return new ClassDto() { Id=entity.Id,Grid=entity.Grid,Name=entity.Name,Days=entity.Days,Students=entity.Students};
        }
    }
}
