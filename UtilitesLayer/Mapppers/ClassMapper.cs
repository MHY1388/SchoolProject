﻿using DataLayer.Entities;
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
        public static Class MapToClass(this CreateClassDto entity)
        {
            return new Class() {Grid=entity.Grid,Name=entity.Name};
        }
        public static Class MapToClass(this ClassDto entity)
        {
            return new Class() {Id=entity.Id,  Grid = entity.Grid, Name = entity.Name, Updated=DateTime.Now };
        }
    }
}
