using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.IO;
using UtilitesLayer.Utilities.ali;

namespace UtilitesLayer.Utilities

{
    public class FileManager
    {
        private readonly CloudTool storage;

        public FileManager(CloudTool storage)
        {
            this.storage = storage;
        }
        public async Task<string> SaveFile(IFormFile file, string filepath, string Bucket, DateTime? time=null)
        {
            
            //string file_dir = Directory.GetCurrentDirectory() + (filepath).Replace("/", "\\");
            //if (!Directory.Exists(file_dir))
            //{
            //    Directory.CreateDirectory(file_dir);
            //}
            //var save_path = new FileStream(file_dir + file_name_path, FileMode.Create);
            //file.CopyTo(save_path);
            //save_path.Close();
            string file_name_path = Guid.NewGuid() + file.FileName;
            await storage.AddItem(file, filepath + file_name_path, Bucket, time);
            return filepath+file_name_path;
        }

        public async Task DeleteFile(string fileName, string filepath, string bucket)
        {
            await storage.DeleteObjectHelper(bucket, filepath + fileName);
        }
    }
}
