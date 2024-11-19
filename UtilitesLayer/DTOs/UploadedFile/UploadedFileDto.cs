using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilitesLayer.DTOs.UploadedFile
{
    public class UploadedFileDto:BaseDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string FilePath { get; set; }
    }
}
