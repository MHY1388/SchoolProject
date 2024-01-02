using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UtilitesLayer.DTOs.Category;
using UtilitesLayer.DTOs.Day;
using UtilitesLayer.DTOs.Global;
using UtilitesLayer.DTOs.Post;
using UtilitesLayer.DTOs.Presence;
using UtilitesLayer.DTOs.Section;
using UtilitesLayer.Mapppers;
using UtilitesLayer.Utilities;
using WebLayer.Data;

namespace UtilitesLayer.Services
{
    public interface ISectionService
    {
        public Task<OperationResult> CreateSection(CreateSectionDto dto);
        public Task<OperationResult> UpdateSection(SectionDto dto);
        public Task<List<SectionDto>> GetSections(int dayid);
        public Task<SectionDto> GetSection(int id);
        public Task<OperationResult> DeleteSection(int sectionid);
        public Task<Paggination<SectionDto>> GetPaggination(int dayid, int page, int pageSize, string name = null);
        public Task<bool> NameExsists(string name, int dayid);
    }
    public class SectionService : ISectionService
    {
        private readonly IGenericRepository<Section> db;
        public SectionService(ApplicationDbContext context)
        {
            db = new GenericRepository<Section>(context);
        }
        public async Task<OperationResult> CreateSection(CreateSectionDto dto)
        {
           return await db.Create(dto.MapToDto());
        }

        public async Task<OperationResult> DeleteSection(int sectionid)
        {
          return await db.Delete(sectionid);
        }

        public async Task<Paggination<SectionDto>> GetPaggination(int dayid, int page, int pageSize, string name = null)
        {
            Paggination<Section> paggination;
            if (!name.IsNullOrEmpty())
            {
                paggination = await db
                   .GetPaggination(pageSize, a => a.DayId==dayid&& a.Name.Contains(name), page);
            }
            else
            {
                paggination = await db.GetPaggination(pageSize,a=>a.DayId==dayid, page);
            }
            return new Paggination<SectionDto>() { CurrentPage = paggination.CurrentPage, GetSize = paggination.GetSize, PageCount = paggination.PageCount, Objects = paggination.Objects.Select(a => a.MapToDto()).ToList() };

        }

        public async Task<SectionDto> GetSection(int id)
        {
            return (await db.Get(id)).MapToDto();
        }

        public async Task<List<SectionDto>> GetSections(int dayid)
        {
            return ( await db.FindAll(a=>a.DayId== dayid)).Select(a=>a.MapToDto()).ToList();
        }

        public async Task<bool> NameExsists(string name, int dayid)
        {
            return await db.Any(a => a.DayId == dayid && a.Name.ToLower() == name.ToLower());
        }

        public async Task<OperationResult> UpdateSection(SectionDto dto)
        {
            return await db.Update(dto.MapToSection());
        }
    }
    public interface IPresenceService
    {
        public Task<OperationResult> CreatePresence(CreatePresenceDto dto);
        public Task<OperationResult> UpdatePresence(int id, bool ispresence);
        public Task<List<PresenceDto>> GetPresences(int Sectionid);
        public Task<Paggination<CustomPresenceDto>> GetPaggination(int page, int pageSize, DateTime date);

    }
    public class PresenceService : IPresenceService
    {
        private readonly IGenericRepository<Presence> db;
        private readonly ApplicationDbContext context;

        public PresenceService(ApplicationDbContext context)
        {
            db = new GenericRepository<Presence>(context);
            this.context = context;
        }
        public async Task<OperationResult> CreatePresence(CreatePresenceDto dto)
        {
           return await db.Create(dto.MapToDto());
        }

        public async Task<Paggination<CustomPresenceDto>> GetPaggination(int page, int pageSize, DateTime date)
        {
            List<Day> days = context.Days.Include(a=>a.DayClass).Where(a=>a.Created.Date== date.Date).ToList();
            List<Section> sections = context.Sections.Where(a=>days.Select(c=>c.Id).Contains(a.DayId)).ToList();
            List<Expression<Func<Presence, dynamic>>> includes = new() { a => a.Student };

            var data = await db.GetPagginationWithInclude(size: pageSize,page: page, expression: a => sections.Select(c=>c.Id).Contains(a.SectionId)&& a.IsPresence==false,includes:includes);
            var result = data.Objects.Select(a =>
            {
                var section = sections.Single(k => k.Id == a.SectionId);
                var day = days.Single(K => K.Id == section.DayId);
                return a.MapToDto(classname: day.DayClass.Grid.ToString() + "-" + day.DayClass.Name, section: section.Name);
            }).ToList();
            return new Paggination<CustomPresenceDto>()
            {
                CurrentPage = data.CurrentPage,
                GetSize = data.GetSize,
                PageCount = data.PageCount,
                Objects = result
            };
        }

        public async Task<List<PresenceDto>> GetPresences(int Sectionid)
        {
            List<Expression<Func<Presence, dynamic>>> a = [a => a.Student];
            return (await db.FindAllWithInclude(expression: a => a.SectionId==Sectionid, includes:a)).Select(m => m.MapToDto()).ToList();
        }

        public async Task<OperationResult> UpdatePresence(int id, bool ispresence)
        {
            Presence? presence = context.Presences.SingleOrDefault(a => a.Id == id);
            if(presence == null)
            {
                return OperationResult.Error();
            }
            presence.IsPresence = ispresence;
            context.Entry(presence).DetectChanges();
            return OperationResult.Success();
        }
    }
}
