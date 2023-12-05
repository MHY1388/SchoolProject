using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilitesLayer.DTOs.Class;
using WebLayer.Data;
using UtilitesLayer.Mapppers;

namespace UtilitesLayer.Services
{
    public interface IClassService
    {
        public Task<ClassDto> GetClass(int classid);
    }
    public class ClassService : IClassService
    {
        private readonly IGenericRepository<Class> db;
        public ClassService(ApplicationDbContext context)
        {
            db = new GenericRepository<Class>(context);
        }
        public async Task<ClassDto> GetClass(int classid)
        {
           return (await db.Get(classid)).MapToDto();
        }
    }
}
