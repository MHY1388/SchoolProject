using DataLayer.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilitesLayer.DTOs.Class;
using UtilitesLayer.DTOs.Day;
using UtilitesLayer.DTOs.Global;
using UtilitesLayer.Utilities;
using UtilitesLayer.Mapppers;
using WebLayer.Data;

namespace UtilitesLayer.Services
{
    public interface IDayService
    {
        public Task<OperationResult> AddDay(string userId);
        public Task<OperationResult> AddDay(int classId);
        public Task<OperationResult> DeleteDay(int DayId);
        public Task<List<DayDto>> GetDays(int classId);
        public Task<DayDto> GetDay(int dayId);
        public Task<bool> DayExsists(DateTime time, int classId);
        public Task<Paggination<DayDto>> GetPaggination(int page, int pageSize, int classId, int? day = null, int? month = null, int? year = null, bool is_bigger = true);
    }
    public class DayService : IDayService
    {
        private readonly IGenericRepository<Day> db;
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> userManager;

        public DayService(ApplicationDbContext context, Microsoft.AspNetCore.Identity.UserManager<User> userManager)
        {
            db = new GenericRepository<Day>(context);
            this.userManager = userManager;
        }

        public async Task<OperationResult> AddDay(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user.ClassId is not null)
            {
                return await db.Create(new Day() {Created= DateTime.Now, classId=(int)user.ClassId});
            }
            return OperationResult.Error("این کاربر وجود ندارد");
        }
        public async Task<OperationResult> AddDay(int classId)
        {
                return await db.Create(new Day() { Created = DateTime.Now, classId = classId });
        }

        public async Task<bool> DayExsists(DateTime time, int classId)
        {
            return await db.Any(o => o.Created.Date == time.Date&&o.classId==classId);
        }

        public async Task<OperationResult> DeleteDay(int DayId)
        {
            return await db.Delete(DayId);
        }

        public async Task<DayDto> GetDay(int dayId)
        {
            return (await db.Get(dayId)).MapToDto();
        }

        public async Task<List<DayDto>> GetDays(int classId)
        {
            return (await db.FindAll(a=>a.classId==classId)).Select(a => a.MapToDto()).ToList();
        }

        public async Task<Paggination<DayDto>> GetPaggination(int page, int pageSize, int classId, int? day = null, int? month = null, int? year = null, bool is_bigger = true)
        {

            //List<Expression<Func<Post, dynamic>>> includes = new() { a => a.Category };
            Paggination<Day> data = null;
            if (day is not null && month is not null && year is not null)
            {
                PersianCalendar pc = new PersianCalendar();
                DateTime dt = new DateTime((int)year, (int)month, (int)day, pc).Date;
                if (is_bigger)
                {

                    data = await db.GetPaggination(size: pageSize, page: page, expression: a => a.classId==classId&&(a.Created.Date >= dt));
                }
                else
                {
                    data = await db.GetPaggination(size: pageSize, page: page, expression: a => a.classId == classId && (a.Created.Date <= dt));

                }
                data.Objects = data.Objects.OrderBy(a => a.Created).ToList();
            }
            else
            {
                data = await db.GetPaggination(size: pageSize, page: page, expression:a => a.classId == classId);
                data.Objects = data.Objects.OrderByDescending(a => a.Created).ToList();

            }
            return new Paggination<DayDto>()
            {
                CurrentPage = data.CurrentPage,
                GetSize = data.GetSize,
                PageCount = data.PageCount,
                Objects = data.Objects.Select(a => a.MapToDto()).ToList()
            };
        }
    }
}
