using DataLayer.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UtilitesLayer.DTOs.Global;
using UtilitesLayer.Utilities;
using WebLayer.Data;

namespace UtilitesLayer.Services
{
    public interface ILessonService
    {
        public Task<OperationResult> Create(string name);
        public Task<bool> NameExsists(string name);
        public Task<OperationResult> Update(int id, string name);
        public Task<OperationResult> Delete(int id);
        public Task<Lesson> Get(int id);
        public Task<ICollection<Lesson>> GetAll();
        public Task<Paggination<Lesson>> GetPaggination(int size, int page = 1);
        public Task<Paggination<Lesson>> GetPaggination(int size, Expression<Func<Lesson, bool>> expression, int page = 1);
        public Task<object> Find(Expression<Func<object, bool>> expression);
        public Task<ICollection<object>> FindAll(Expression<Func<object, bool>> expression);
    }
    public class LessonService : ILessonService
    {
        private readonly IGenericRepository<Lesson> db;
        public LessonService(ApplicationDbContext dbContext)
        {
            db = new GenericRepository<Lesson>(dbContext);
        }
        public async Task<OperationResult> Create(string name)
        {
            if (name.IsNullOrEmpty())
                return OperationResult.Error();
            return await db.Create(new Lesson() { Name = name });
        }

        public  async Task<OperationResult> Delete(int id)
        {
            if (id == 0)
                return OperationResult.Error();
            return await db.Delete(id);
        }

        public Task<object> Find(Expression<Func<object, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<object>> FindAll(Expression<Func<object, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public async Task<Lesson> Get(int id)
        {
            return await db.GetNoTracking(id);
        }

        public async Task<ICollection<Lesson>> GetAll()
        {
            return await db.GetAll();
        }

        public Task<Paggination<Lesson>> GetPaggination(int size, int page = 1)
        {
            return db.GetPaggination(size, page);
        }

        public async Task<Paggination<Lesson>> GetPaggination(int size, Expression<Func<Lesson, bool>> expression, int page = 1)
        {
            return await db.GetPaggination(size, expression, page);
        }

        public Task<bool> NameExsists(string name)
        {
            return db.Any(a=>a.Name==name);
        }

        public async Task<OperationResult> Update(int id, string name)
        {
            if (id == 0 || name.IsNullOrEmpty())
                return OperationResult.Error();
            return await db.Update(new Lesson() { Id = id, Name = name });
        }
    }
}
