using DataLayer.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilitesLayer.DTOs.UploadedFile;
using WebLayer.Data;
using UtilitesLayer.Mapppers;
using UtilitesLayer.Utilities;
using Microsoft.IdentityModel.Tokens;
using UtilitesLayer.DTOs.Category;
using UtilitesLayer.DTOs.Global;
namespace UtilitesLayer.Services
{
    public class UploadedFileService : IUploadedFileService
    {
        private readonly IGenericRepository<UploadedFile> _repository;
        private readonly ApplicationDbContext context;
        private readonly FileManager fileManager;

        public UploadedFileService(ApplicationDbContext context,FileManager fileManager)
        {
            _repository = new GenericRepository<UploadedFile>(context);
            this.context = context;
            this.fileManager = fileManager;
        }

        public async Task<OperationResult> DeleteFile(string file_name)
        {
            var file = await _repository.Find(a=>a.Name == file_name);
            if (file != null)
            {
                await fileManager.DeleteFile(file.FilePath, DirectoryPath.UploadedFiles, DirectoryPath.BucketName);
                return await _repository.Delete(file.Id);
            }
            return OperationResult.Error();
        }

        public async Task<List<UploadedFileDto>> GetFiles()
        {
            var files = await _repository.GetAll();
            return files.Select(a=>a.MapToDto()).ToList();
        }
        public async Task<Paggination<UploadedFileDto>> GetPaggination(int page, int pageSize, string name = null)
        {
            Paggination<UploadedFile> paggination;
            if (!name.IsNullOrEmpty())
            {
                paggination = await _repository
                   .GetPaggination(pageSize, a => a.Name.Contains(name), page);
            }
            else
            {
                paggination = await _repository.GetPaggination(pageSize, page);
            }
            return new Paggination<UploadedFileDto>() { CurrentPage = paggination.CurrentPage, GetSize = paggination.GetSize, PageCount = paggination.PageCount, Objects = paggination.Objects.Select(a => a.MapToDto()).ToList() };
        }

        public async Task<OperationResult> UploadFile(string file_name, IFormFile file)
        {
            int lenght = Convert.ToInt16(file.Length / 1024 / 1024);
            string filepath = await fileManager.SaveFile(file,DirectoryPath.UploadedFiles,DirectoryPath.BucketName);
            return await _repository.Create(new() { FilePath = filepath ,Name=file_name,Length=lenght,ContentType=file.ContentType});
        }

        public async Task<bool> NameExists(string name)
        {
            return await _repository.Any(r => r.Name == name);
        }

        public async Task<OperationResult> DeleteFile(int Id)
        {
            var file = await _repository.Get(Id);
            if (file != null)
            {
                await fileManager.DeleteFile(file.FilePath, DirectoryPath.UploadedFiles, DirectoryPath.BucketName);
                return await _repository.Delete(file.Id);
            }
            return OperationResult.Error();
        }
    }
}
