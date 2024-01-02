using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilitesLayer.DTOs.Teacher;

namespace UtilitesLayer.Mapppers
{
    public static class TeacherMapper
    {
        public static Teacher MapToTeacher(this CreateTeacherDto teacher, string filename)
        {
            return new Teacher() { Name = teacher.Name, Description = teacher.Description, Doc = teacher.Doc, PhoneNumber = teacher.PhoneNumber, PublicPhoneNumber = teacher.PublicPhoneNumber, FileName=filename };
        }
        public static Teacher MapToTeacher(this TeacherDto teacher)
        {
            return new Teacher() {Id=teacher.Id,  Name = teacher.Name, Description = teacher.Description, Doc = teacher.Doc, PhoneNumber = teacher.PhoneNumber, PublicPhoneNumber = teacher.PublicPhoneNumber };
        }
        public static TeacherDto MapToDto(this Teacher teacher)
        {
            var data =  new TeacherDto() {FilePath = teacher.FileName, Name = teacher.Name, Description = teacher.Description, Doc = teacher.Doc, PhoneNumber = teacher.PhoneNumber, PublicPhoneNumber = teacher.PublicPhoneNumber };
            data = BaseMapper.BaseMap(teacher, data);
            return data;
        }

    }
}
