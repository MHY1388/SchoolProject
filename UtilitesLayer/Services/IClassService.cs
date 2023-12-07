using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilitesLayer.DTOs.Class;
using WebLayer.Data;
using UtilitesLayer.Mapppers;
using UtilitesLayer.Utilities;
using UtilitesLayer.DTOs.Global;
using System.Linq.Expressions;
using UtilitesLayer.DTOs.Post;
using Microsoft.IdentityModel.Tokens;

namespace UtilitesLayer.Services
{
    public interface IClassService
    {
        public Task<OperationResult> AddClass(CreateClassDto createClassDto);
        public Task<OperationResult> DeleteClass(int classId);
        public Task<List<ClassDto>> GetClasses();
        public Task<ClassDto> GetClass(int classid);
        public Task< Paggination<ClassDto>> GetPaggination(int page, int pageSize, string name);
        public Task<OperationResult> UpdateClass(ClassDto classDto);
    }
    public class ClassService : IClassService
    {
        private readonly IGenericRepository<Class> db;
        public ClassService(ApplicationDbContext context)
        {
            db = new GenericRepository<Class>(context);
        }

        public async Task<OperationResult> AddClass(CreateClassDto createClassDto)
        {
           return await db.Create(createClassDto.MapToClass());
        }

        public async Task<OperationResult> DeleteClass(int classId)
        {
            return await db.Delete(classId);
        }

        public async Task<ClassDto> GetClass(int classid)
        {
           return (await db.Get(classid)).MapToDto();
        }

        public async Task<List<ClassDto>> GetClasses()
        {
          return ( await db.GetAll()).Select(a=>a.MapToDto()).ToList();
        }

        public async Task<Paggination<ClassDto>> GetPaggination(int page, int pageSize, string name)
        {
            //List<Expression<Func<Post, dynamic>>> includes = new() { a => a.Category };
            Paggination<Class> data = null;
            if(!name.IsNullOrEmpty()) 
            {
             data = await db.GetPaggination(size: pageSize, page: page, expression: a => ( a.Grid.ToString() +" - "+a.Name).Contains(name));
            }
            else
            {
                data = await db.GetPaggination(size: pageSize, page: page);

            }
            return new Paggination<ClassDto>()
            {
                CurrentPage = data.CurrentPage,
                GetSize = data.GetSize,
                PageCount = data.PageCount,
                Objects = data.Objects.Select(a => a.MapToDto()).ToList()
            };
        }

        public async Task<OperationResult> UpdateClass(ClassDto classDto)
        {
           return await db.Update(classDto.MapToClass());
        }
    }
}
