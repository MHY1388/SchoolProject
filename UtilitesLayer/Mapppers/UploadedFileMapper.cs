using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilitesLayer.DTOs.UploadedFile;

namespace UtilitesLayer.Mapppers
{
    public static class UploadedFileMapper
    {
        public static UploadedFileDto MapToDto(this UploadedFile file)
        {
            var new_file = new UploadedFileDto() { Name = file.Name ,FilePath=file.FilePath,Lenght=file.Length,ContentType=file.ContentType};
            return BaseMapper.BaseMap(file, new_file);
        }
    }
}
