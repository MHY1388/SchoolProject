using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilitesLayer.DTOs.HomeWork;
using UtilitesLayer.Utilities;

namespace UtilitesLayer.Mapppers
{
    public static class HomeWorkMapper
    {
        public static HomeWork MapToHomeWork(this HomeWorkDto model)
        {
            var data = new HomeWork() {Id=model.Id, Description = model.Description, Type = model.Type, LessonId = model.LessonId, ClassId = model.ClassId };
            try
            {
                string[] strings = model.LastTimeStr.Split("/");
                PersianCalendar pc = new PersianCalendar();
                var date = new DateTime(year: Convert.ToInt32(strings[0].PersianToEnglish()), month: Convert.ToInt32(strings[1].PersianToEnglish()), day: Convert.ToInt32(strings[2].PersianToEnglish()), pc).Date;
                data.LastTime = date;
            }
            catch
            {

                return null;
            }
            return data;
        }
        public static HomeWorkDto MapToDto(this HomeWork model)
        {
            var data = new HomeWorkDto() { Description = model.Description, Type = model.Type, LastTime = model.LastTime, LessonId = model.LessonId, ClassId = model.ClassId };
            if (model.Lesson is not null)
                data.Lesson = model.Lesson;
            if (model.Class is not null)
                data.Class = model.Class.MapToDto();
            data.LastTimeStr = model.LastTime.ToString("yyyy/MM/dd");
            data = BaseMapper.BaseMap(model, data);
            return data;
        }
        public static HomeWork MapToHomeWork(this CreateHomeWorkDto model)
        {
            var data= new HomeWork() { Description = model.Description, ClassId = model.ClassId, LessonId = model.LessonId, Type = model.Type };
            try
            {
                string[] strings = model.LastTime.Split("/");
                PersianCalendar pc = new PersianCalendar();
                var date = new DateTime(year: Convert.ToInt32(strings[0].PersianToEnglish()),month: Convert.ToInt32(strings[1].PersianToEnglish()),day: Convert.ToInt32(strings[2].PersianToEnglish()),pc).Date;
                data.LastTime = date;
            }
            catch
            {
                
                return null;
            }
            return data;
        }
    }
}
