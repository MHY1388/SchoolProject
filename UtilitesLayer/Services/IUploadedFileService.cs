using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilitesLayer.DTOs.UploadedFile;
using UtilitesLayer.Utilities;

namespace UtilitesLayer.Services
{
    public interface IUploadedFileService
    {
        public Task<OperationResult> UploadFile(string file_name, IFormFile file);
        public Task<OperationResult> DeleteFile(string file_name);
        public Task<List<UploadedFileDto>> GetFiles();
        public Task<bool> NameExists(string name);
    }
}
