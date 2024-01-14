using DataLayer.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UtilitesLayer.DTOs.Category;
using UtilitesLayer.DTOs.Global;
using UtilitesLayer.DTOs.HomeWork;
using UtilitesLayer.Mapppers;
using UtilitesLayer.Utilities;
using WebLayer.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UtilitesLayer.Services
{
    public interface IHomeWorkService
    {
        public Task<OperationResult> Create(CreateHomeWorkDto model);
        public Task<OperationResult> Update(HomeWorkDto model);
        public Task<OperationResult> Delete(int Id);
        public Task<ICollection<HomeWorkDto>> GetAll(int classId);
        public Task<HomeWorkDto> Get(int id);
        public Task<Paggination<HomeWorkDto>> GetPaggination(int page, int pageSize, int classId, string? name = null, HomeWorkType? type = null, int? day = null, int? month = null, int? year = null, bool is_bigger = true);

    }
    public class HomeWorkService : IHomeWorkService
    {
        private readonly IGenericRepository<HomeWork> db;
        public HomeWorkService(ApplicationDbContext context)
        {
            db = new GenericRepository<HomeWork>(context);
        }
        public async Task<OperationResult> Create(CreateHomeWorkDto model)
        {
            return await db.Create(model.MapToHomeWork());
        }

        public async Task<OperationResult> Delete(int Id)
        {
            return await db.Delete(Id);
        }

        public async Task<HomeWorkDto> Get(int id)
        {
            return (await db.Get(id)).MapToDto() ;
        }

        public async Task<ICollection<HomeWorkDto>> GetAll(int classId)
        {
            return (await db.FindAll(a=>a.ClassId == classId)).Select(a => a.MapToDto()).ToList();
        }

        public async Task<Paggination<HomeWorkDto>> GetPaggination(int page, int pageSize,int classId, string? name = null, HomeWorkType? type = null, int? day = null, int? month = null, int? year = null, bool is_bigger = true)
        {
            Paggination<HomeWork> paggination;
            List<Expression<Func<HomeWork, dynamic>>> includes = new() { a => a.Lesson };

            if (day is not null && month is not null && year is not null)
            {
                if ((!name.IsNullOrEmpty()) || (type is not null))
                {
                    PersianCalendar pc = new PersianCalendar();
                    DateTime dt = new DateTime((int)year, (int)month, (int)day, pc).Date;
                    if (is_bigger)
                    {

                        paggination = await db.GetPagginationWithIncludeSort(size: pageSize, expression: a => a.ClassId == classId && (a.LastTime.Date >= dt) && (a.Lesson.Name.Contains(name) || a.Type == type),includes,sort:a=>a.LastTime,bigger:false, page: page);
                    }
                    else
                    {
                        paggination = await db.GetPagginationWithIncludeSort(size: pageSize, expression: a => a.ClassId == classId && (a.LastTime.Date <= dt) && (a.Lesson.Name.Contains(name) || a.Type == type), includes, sort: a => a.LastTime, bigger: true, page: page);

                    }
                }
                else
                {
                    PersianCalendar pc = new PersianCalendar();
                    DateTime dt = new DateTime((int)year, (int)month, (int)day, pc).Date;
                    if (is_bigger)
                    {

                        paggination = await db.GetPagginationWithIncludeSort(size: pageSize, expression: a => a.ClassId == classId && (a.LastTime.Date >= dt), includes, sort: a => a.LastTime, bigger: false, page: page);
                    }
                    else
                    {
                        paggination = await db.GetPagginationWithIncludeSort(size: pageSize, expression: a => a.ClassId == classId && (a.LastTime.Date <= dt), includes, sort: a => a.LastTime, bigger: true, page: page);

                    }
                }
               
            }
            else
            {
                if ((!name.IsNullOrEmpty()) || (type is not null))
                {
                    paggination = await db
                       .GetPagginationWithIncludeSort(pageSize, a => a.ClassId == classId && (a.Lesson.Name.Contains(name) || a.Type == type) && a.LastTime > DateTime.Now.Date.AddDays(-1), includes, sort: a => a.LastTime, bigger: false, page: page);
                }
                else
                {
                    paggination = await db.GetPagginationWithIncludeSort(pageSize, a => a.ClassId == classId && a.LastTime >DateTime.Now.Date.AddDays(-1), includes, sort: a => a.LastTime, bigger: false, page: page);
                }
            }

            return new Paggination<HomeWorkDto>() { CurrentPage = paggination.CurrentPage, GetSize = paggination.GetSize, PageCount = paggination.PageCount, Objects = paggination.Objects.Select(a => a.MapToDto()).ToList() };

        }

        public async Task<OperationResult> Update(HomeWorkDto model)
        {
           return await db.Update(model.MapToHomeWork());
        }
    }
}
