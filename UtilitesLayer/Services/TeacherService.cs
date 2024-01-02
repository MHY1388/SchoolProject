using DataLayer.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilitesLayer.DTOs.Category;
using UtilitesLayer.DTOs.Global;
using UtilitesLayer.DTOs.Teacher;
using UtilitesLayer.Mapppers;
using UtilitesLayer.Utilities;
using WebLayer.Data;

namespace UtilitesLayer.Services
{
    public interface ITeacherService
    {
        public Task<OperationResult> CreateTecher(CreateTeacherDto model);
        public Task<TeacherDto> GetTecher(int Id);
        public Task<List<TeacherDto>> GetTechers();
        public Task<OperationResult> UpdateTecher(TeacherDto model);
        public Task<OperationResult> DeleteTeacher(int Id);
        public Task<Paggination<TeacherDto>> GetPaggination(int page, int pageSize, string name = null);

    }
    public class TeacherService : ITeacherService
    {
        private readonly ApplicationDbContext context;
        private readonly FileManager fileManager;
        private readonly IGenericRepository<Teacher> db;

        public TeacherService(ApplicationDbContext context, FileManager fileManager)
        {
            this.context = context;
            this.fileManager = fileManager;
            this.db = new GenericRepository<Teacher>(context);
        }
        public async Task<OperationResult> CreateTecher(CreateTeacherDto model)
        {
           var result = await fileManager.SaveFile(model.Image, DirectoryPath.TeacherImages, DirectoryPath.BucketName);
           return await db.Create(model.MapToTeacher(result));
        }

        public async Task<OperationResult> DeleteTeacher(int Id) 
        {
            Teacher teacher = await db.Get(Id);
            fileManager.DeleteFile()
            return await db.Delete(teacher);
        }

        public async Task<Paggination<TeacherDto>> GetPaggination(int page, int pageSize, string name = null)
        {
            Paggination<Teacher> paggination;
            if (!name.IsNullOrEmpty())
            {
                paggination = await db
                   .GetPaggination(pageSize, a => a.Name.Contains(name), page);
            }
            else
            {
                paggination = await db.GetPaggination(pageSize, page);
            }
            return new Paggination<TeacherDto>() { CurrentPage = paggination.CurrentPage, GetSize = paggination.GetSize, PageCount = paggination.PageCount, Objects = paggination.Objects.Select(a => a.MapToDto()).ToList() };
        }

        public async Task<TeacherDto> GetTecher(int Id)
        {
            return (await db.Get(Id))?.MapToDto();
        }

        public async Task<List<TeacherDto>> GetTechers()
        {
            return (await db.GetAll()).Select(a=>a.MapToDto()).ToList();
        }

        public Task<OperationResult> UpdateTecher(TeacherDto model) => db.Update(model.MapToTeacher());
    }
}
