using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace UtilitesLayer.Utilities
{
    public static class DirectoryPath
    {
        public const string MediaImages = "\\wwwroot\\media\\images\\";
        public const string MediaImagesContent = "\\wwwroot\\media\\images\\content\\";
        public static string GetImageUrl(string name)
        {
            return MediaImages.Replace("\\", "/").Replace("/wwwroot", "") + name;
        }
    }
   
}
