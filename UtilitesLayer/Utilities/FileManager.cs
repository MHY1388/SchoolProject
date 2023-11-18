using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.IO;

namespace UtilitesLayer.Utilities

{
    public static class FileManager
    {
        public static string SaveFile(IFormFile file, string filepath)
        {
            string file_name_path = Guid.NewGuid() + file.FileName;
            string file_dir = Directory.GetCurrentDirectory() + (filepath).Replace("/", "\\");
            if (!Directory.Exists(file_dir))
            {
                Directory.CreateDirectory(file_dir);
            }
            var save_path = new FileStream(file_dir + file_name_path, FileMode.Create);
            file.CopyTo(save_path);
            save_path.Close();
            return file_name_path.Replace("\\","/");
        }
        public static bool DeleteFile(string fileName, string filepath)
        {
            if (!Directory.Exists(filepath)) 
            {
                try
                {
                    var a = Path.Combine(filepath, fileName);
                    File.Delete((Directory.GetCurrentDirectory()+ a).Replace("/","\\"));
                    return true; 
                }catch
                {
                    return false;
                }
            }
            return true;
        }
    }
}
